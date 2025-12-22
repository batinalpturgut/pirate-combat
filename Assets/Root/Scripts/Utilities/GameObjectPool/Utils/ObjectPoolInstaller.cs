using System;
using Root.Scripts.Utilities.GameObjectPool.Enums;
using UnityEngine;

namespace Root.Scripts.Utilities.GameObjectPool.Utils
{
    [DisallowMultipleComponent]
    public class ObjectPoolInstaller : MonoBehaviour
    {
        [SerializeField] private ObjectConfiguration[] objectConfigurations;

        [Serializable]
        private struct ObjectConfiguration
        {
            [field: SerializeField] 
            public GameObject GameObject { get; private set; }
            [field: SerializeField] 
            public EPrewarmTime PrewarmTime { get; private set; }
            [field: SerializeField] 
            public PoolSettings PoolSettings { get; private set; }
        }

        private void Awake()
        {
            foreach (ObjectConfiguration objectConfiguration in objectConfigurations)
            {
                if (objectConfiguration.PrewarmTime == EPrewarmTime.Awake)
                {
                    ObjectPoolController.CreatePool(objectConfiguration.GameObject, objectConfiguration.PoolSettings);
                }
            }
        }

        private void Start()
        {
            foreach (ObjectConfiguration objectConfiguration in objectConfigurations)
            {
                if (objectConfiguration.PrewarmTime == EPrewarmTime.Start)
                {
                    ObjectPoolController.CreatePool(objectConfiguration.GameObject, objectConfiguration.PoolSettings);
                }
            }
        }

        private void OnValidate()
        {
            gameObject.name = $"{nameof(ObjectPoolInstaller)}";
            transform.SetPositionAndRotation(default, default);
        }
    }
}