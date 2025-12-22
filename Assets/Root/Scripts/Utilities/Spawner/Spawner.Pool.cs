using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.GameObjectPool;
using Root.Scripts.Utilities.GameObjectPool.Utils;
using UnityEngine;

namespace Root.Scripts.Utilities.Spawner
{
    public static partial class Spawner
    {
        public static T Get<T, T1, T2, T3, T4>(Transform obj, Vector3 worldPos, Quaternion rotation, T1 t1, T2 t2,
            T3 t3, T4 t4, PoolSettings poolSettings = null) where T : IInitializer<T1, T2, T3, T4>
        {
            if (poolSettings != null)
            {
                ObjectPoolController.CreatePool(obj.gameObject, poolSettings);
            }

            Transform transform = ObjectPoolController.Get(obj.gameObject, worldPos, rotation).transform;

            T t = transform.GetComponent<T>();
            t.Initialize(t1, t2, t3, t4);
            return t;
        }

        public static T Get<T, T1, T2, T3>(Transform obj, Vector3 worldPos, Quaternion rotation, T1 t1, T2 t2, T3 t3,
            PoolSettings poolSettings = null) where T : IInitializer<T1, T2, T3>
        {
            if (poolSettings != null)
            {
                ObjectPoolController.CreatePool(obj.gameObject, poolSettings);
            }

            Transform transform = ObjectPoolController.Get(obj.gameObject, worldPos, rotation).transform;

            T t = transform.GetComponent<T>();
            t.Initialize(t1, t2, t3);
            return t;
        }

        public static T Get<T, T1, T2>(Transform obj, Vector3 worldPos, Quaternion rotation, T1 t1, T2 t2,
            PoolSettings poolSettings = null) where T : IInitializer<T1, T2>
        {
            if (poolSettings != null)
            {
                ObjectPoolController.CreatePool(obj.gameObject, poolSettings);
            }

            Transform transform = ObjectPoolController.Get(obj.gameObject, worldPos, rotation).transform;

            T t = transform.GetComponent<T>();
            t.Initialize(t1, t2);
            return t;
        }

        public static T Get<T, T1>(Transform obj, Vector3 worldPos, Quaternion rotation, T1 t1,
            PoolSettings poolSettings = null) where T : IInitializer<T1>
        {
            if (poolSettings != null)
            {
                ObjectPoolController.CreatePool(obj.gameObject, poolSettings);
            }

            Transform transform = ObjectPoolController.Get(obj.gameObject, worldPos, rotation).transform;

            T t = transform.GetComponent<T>();
            t.Initialize(t1);
            return t;
        }

        public static T Get<T>(Transform obj, Vector3 worldPos, Quaternion rotation, PoolSettings poolSettings = null)
        {
            if (poolSettings != null)
            {
                ObjectPoolController.CreatePool(obj.gameObject, poolSettings);
            }

            Transform transform = ObjectPoolController.Get(obj.gameObject, worldPos, rotation).transform;

            if (typeof(T) == typeof(Transform))
            {
                return (T)(object)transform;
            }

            T t = transform.GetComponent<T>();
            return t;
        }
    }
}