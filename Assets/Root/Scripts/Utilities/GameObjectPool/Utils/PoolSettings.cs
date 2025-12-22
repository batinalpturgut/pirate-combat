using System;
using Root.Scripts.Utilities.GameObjectPool.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Root.Scripts.Utilities.GameObjectPool.Utils
{
    [Serializable]
    public class PoolSettings
    {
        [field: SerializeField] 
        public int DefaultCapacity { get; set; } = 5;
        [field: SerializeField] 
        public int MaxCapacity { get; set; } = 15;
        [field: SerializeField] 
        public bool Expandable { get; set; } = true;
        [field: SerializeField]
        public ECallbackSearchType CallbackSearchType { get; set; } = ECallbackSearchType.GetComponents;
        [field: SerializeField]
        public DelegateCallbacks DelegateCallbacks { get; set; }

    }
    
    [Serializable]
    public class DelegateCallbacks
    {
        [field: SerializeField]
        public UnityEvent<GameObject> OnGet { get; set; } = new UnityEvent<GameObject>();
        [field: SerializeField]
        public UnityEvent<GameObject> OnRelease { get; set; } = new UnityEvent<GameObject>();
    }
}