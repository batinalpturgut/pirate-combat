using System.Collections.Generic;
using Root.Scripts.Utilities.GameObjectPool.Enums;
using Root.Scripts.Utilities.GameObjectPool.Utils;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Utilities.GameObjectPool
{
    public class ObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Queue<GameObject> _objects;
        private PoolSettings _settings;
        private int _totalCount;

        public ECallbackSearchType CallbackSearchType => _settings.CallbackSearchType;
        public DelegateCallbacks DelegateCallbacks => _settings.DelegateCallbacks;
        public Quaternion InitialRotation { get; private set; }
        public Vector3 InitialScale { get; private set; }

        public ObjectPool(GameObject prefab, PoolSettings settings)
        {
            _prefab = prefab;
            _settings = settings;
            _objects = new Queue<GameObject>(settings.DefaultCapacity);

            for (int i = 0; i < settings.DefaultCapacity; i++)
            {
                GameObject obj = InstantiateNewObject();
                _objects.Enqueue(obj);
            }

            _totalCount = settings.DefaultCapacity;
            
            InitialRotation = _prefab.transform.rotation;
            InitialScale = _prefab.transform.localScale;
        }

        public GameObject GetObject(out bool capacityReached)
        {
            if (_objects.Count > 0)
            {
                capacityReached = false;
                return _objects.Dequeue();
            }

            if (_settings.Expandable && _totalCount < _settings.MaxCapacity)
            {
                GameObject obj = InstantiateNewObject();
                _totalCount++;
                capacityReached = false;
                return obj;
            }

            if (_settings.Expandable && _totalCount >= _settings.MaxCapacity)
            {
                GameObject obj = InstantiateNewObject();
                _totalCount++;
                capacityReached = true;
                return obj;
            }

            Log.Console($"{nameof(ObjectPool)}: Pool capacity reached. Returning null.", LogType.Warning);
            capacityReached = false;
            return null;
        }

        public void ReturnObject(GameObject obj)
        {
            if (_objects.Count < _settings.MaxCapacity)
            {
                _objects.Enqueue(obj);
            }
            else
            {
                Object.Destroy(obj);
                _totalCount--;
            }
        }

        public void ClearPool()
        {
            while (_objects.Count > 0)
            {
                GameObject obj = _objects.Dequeue();
                Object.Destroy(obj);
            }

            _totalCount = 0;
        }
        
        public void UpdatePoolSettings(PoolSettings settings)
        {
            _settings = settings;
        }

        private GameObject InstantiateNewObject()
        {
            GameObject obj = Object.Instantiate(_prefab);
            obj.SetActive(false);
            return obj;
        }
    }
}