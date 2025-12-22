using System;
using System.Collections;
using System.Collections.Generic;
using Root.Scripts.Utilities.GameObjectPool.Abstractions;
using Root.Scripts.Utilities.GameObjectPool.Enums;
using Root.Scripts.Utilities.GameObjectPool.Utils;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;
using Object = UnityEngine.Object;

namespace Root.Scripts.Utilities.GameObjectPool
{
    public static class ObjectPoolController
    {
        private static readonly Dictionary<GameObject, ObjectMeta> CloneMetaDict =
            new Dictionary<GameObject, ObjectMeta>();

        private static readonly Dictionary<GameObject, ObjectPool> CloneObjectPoolDict =
            new Dictionary<GameObject, ObjectPool>();

        private static readonly List<ReleaseRequest> ReleaseRequests = new List<ReleaseRequest>();
        private static PoolSettings _globalSettings = new PoolSettings();

        static ObjectPoolController()
        {
            StaticCoroutineRunner.DirectStartCoroutine(Tick());
        }

        private static ObjectPool GetPool(GameObject prefab)
        {
            if (CloneObjectPoolDict.TryGetValue(prefab, out ObjectPool pool))
            {
                return pool;
            }

            Log.Console(
                $"{nameof(ObjectPool)}: Object ({prefab}) not found in pool. New pool creating with default settings.",
                LogType.Warning);
            pool = CreatePool(prefab, _globalSettings);

            return pool;
        }

        public static ObjectPool CreatePool(GameObject prefab, PoolSettings settings = null)
        {
            ObjectPool pool = new ObjectPool(prefab, settings ?? _globalSettings);
            CloneObjectPoolDict[prefab] = pool;
            return pool;
        }

        public static GameObject Get(GameObject prefab, Vector3 position = default, Quaternion rotation = default,
            Transform parent = null)
        {
            if (SystemStatus.IsApplicationQuitting)
            {
                Log.Console($"{nameof(ObjectPool)}: Application is quitting. Cannot get object!", LogType.Warning);
                return null;
            }

            ObjectPool pool = GetPool(prefab);
            GameObject poolObject = pool.GetObject(out bool capacityReached);

            if (poolObject == null)
            {
                return null;
            }

            Transform transform = poolObject.transform;

            transform.SetParent(parent);
            transform.SetPositionAndRotation(position, rotation);
            poolObject.SetActive(true);

            ObjectMeta objectMeta;

            if (CloneMetaDict.TryGetValue(poolObject, out ObjectMeta clone))
            {
                objectMeta = clone;
            }
            else
            {
                objectMeta = new ObjectMeta();
            }

            objectMeta.GameObject = poolObject;
            objectMeta.Pool = pool;
            objectMeta.Status = capacityReached ? EObjectStatus.ActiveOverCapacity : EObjectStatus.Active;
            objectMeta.CachedCallbacks ??= pool.CallbackSearchType switch
            {
                ECallbackSearchType.None => Array.Empty<IPoolable>(),
                ECallbackSearchType.GetComponent => new IPoolable[] { poolObject.GetComponent<IPoolable>() },
                ECallbackSearchType.GetComponents => poolObject.GetComponents<IPoolable>(),
                ECallbackSearchType.GetComponentsInChildren => poolObject.GetComponentsInChildren<IPoolable>(),
                _ => objectMeta.CachedCallbacks
            };

            if (objectMeta.CachedCallbacks is not { Length: > 0 })
            {
                objectMeta.CachedCallbacks = Array.Empty<IPoolable>();
            }

            InvokeGetCallbacks(objectMeta);
            CloneMetaDict[poolObject] = objectMeta;

            return poolObject;
        }

        public static void Release(GameObject clone, bool resetRotationAndScale = true, float delay = 0f)
        {
            if (CloneMetaDict.TryGetValue(clone, out ObjectMeta objectMeta))
            {
                if (objectMeta.Status == EObjectStatus.Inactive)
                {
                    Log.Console($"{nameof(ObjectPool)}: Object is already released and cannot be released again.",
                        LogType.Warning);
                    return;
                }

                if (delay > 0f)
                {
                    ReleaseRequests.Add(new ReleaseRequest
                    {
                        ObjectMeta = objectMeta,
                        TimeRemaining = delay,
                        ResetRotationAndScale = resetRotationAndScale,
                    });
                }
                else
                {
                    ReleaseImmediate(objectMeta, resetRotationAndScale);
                }
            }
            else
            {
                Log.Console($"{nameof(ObjectPool)}: Object is not managed by the pool. Destroying it.",
                    LogType.Warning);
                Object.Destroy(clone, delay);
            }
        }

        public static void UpdateGlobalSettings(PoolSettings settings)
        {
            _globalSettings = settings;
        }

        public static void UpdatePoolSettings(GameObject prefab, PoolSettings settings)
        {
            ObjectPool pool = GetPool(prefab);
            pool.UpdatePoolSettings(settings);
        }

        public static void ClearPool(GameObject clone)
        {
            CloneObjectPoolDict[clone].ClearPool();
            CloneObjectPoolDict.Remove(clone);
            ObjectMeta objectMeta = CloneMetaDict[clone];

            for (int index = ReleaseRequests.Count - 1; index >= 0; index++)
            {
                ReleaseRequest releaseRequest = ReleaseRequests[index];
                if (releaseRequest.ObjectMeta == objectMeta)
                {
                    ReleaseRequests.RemoveAt(index);
                }
            }

            CloneMetaDict.Remove(clone);
        }

        public static void ClearAllPools()
        {
            foreach (ObjectPool pool in CloneObjectPoolDict.Values)
            {
                pool.ClearPool();
            }

            CloneObjectPoolDict.Clear();
            CloneMetaDict.Clear();
            ReleaseRequests.Clear();
        }

        private static IEnumerator Tick()
        {
            for (int index = ReleaseRequests.Count - 1; index >= 0; index--)
            {
                ReleaseRequest request = ReleaseRequests[index];
                request.TimeRemaining -= Time.deltaTime;
                if (request.TimeRemaining <= 0f)
                {
                    ReleaseImmediate(request.ObjectMeta, request.ResetRotationAndScale);
                    ReleaseRequests.RemoveAt(index);
                }
            }

            yield return null;
        }


        private static void ReleaseImmediate(ObjectMeta objectMeta, bool resetRotationAndScale)
        {
            InvokeReleaseCallbacks(objectMeta);
            objectMeta.GameObject.SetActive(false);
            objectMeta.Pool.ReturnObject(objectMeta.GameObject);
            objectMeta.Status = EObjectStatus.Inactive;

            if (resetRotationAndScale)
            {
                objectMeta.GameObject.transform.rotation = objectMeta.Pool.InitialRotation;
                objectMeta.GameObject.transform.localScale = objectMeta.Pool.InitialScale;
            }
        }

        private static void InvokeGetCallbacks(ObjectMeta objectMeta)
        {
            foreach (IPoolable callback in objectMeta.CachedCallbacks)
            {
                callback.OnGet();
            }
            
            objectMeta.Pool.DelegateCallbacks.OnGet.Invoke(objectMeta.GameObject);
        }

        private static void InvokeReleaseCallbacks(ObjectMeta objectMeta)
        {
            foreach (IPoolable callback in objectMeta.CachedCallbacks)
            {
                callback.OnRelease();
            }
            
            objectMeta.Pool.DelegateCallbacks.OnRelease.Invoke(objectMeta.GameObject);
        }


        private class ObjectMeta
        {
            public GameObject GameObject { get; set; }
            public ObjectPool Pool { get; set; }
            public EObjectStatus Status { get; set; }
            public IPoolable[] CachedCallbacks { get; set; }
        }

        private class ReleaseRequest
        {
            public ObjectMeta ObjectMeta { get; set; }
            public float TimeRemaining { get; set; }
            public bool ResetRotationAndScale { get; set; }
        }
    }
}