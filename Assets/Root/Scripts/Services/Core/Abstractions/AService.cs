using Root.Scripts.Extensions;
using UnityEngine;

namespace Root.Scripts.Services.Core.Abstractions
{
    public abstract class AService : ScriptableObject
    {
        public bool IsInitialized { get; protected set; }
        protected AContext GameServicesContext { get; private set; }
        public void Initialize(AContext gameServicesContext)
        {
            GameServicesContext = gameServicesContext;
            OnInitialize();
            GameServicesContext.WaitUntil(this, 
                self => self.IsInitialized, self => self.OnStart());
        }
        protected abstract void OnInitialize();
        protected abstract void OnStart();

        private void OnDisable()
        {
            IsInitialized = false;
            GameServicesContext = null;
        }
    }
}