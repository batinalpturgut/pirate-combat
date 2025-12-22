using Root.Scripts.Services.RemoteConfig.Abstractions;
using Root.Scripts.Utilities.Abstractions;
using UnityEngine;

namespace Root.Scripts.Services.Ads.ScriptableObjects
{
    public class AdConfig : ScriptableObject, IInitializer<IRemoteConfigService>
    {
        [field: SerializeField] 
        private string AndroidId { get; set; } = "ca-app-pub-3940256099942544/6300978111";

        [field: SerializeField] 
        private string IosId { get; set; } = "ca-app-pub-3940256099942544/2934735716";

        [field: SerializeField, Range(0, 60)] 
        private int RetryInterval { get; set; } = 5;

        protected IRemoteConfigService RemoteConfigService;

        public void Initialize(IRemoteConfigService remoteConfigService)
        {
            RemoteConfigService = remoteConfigService;
        }
        
        public string GetId()
        {
#if UNITY_ANDROID
            return AndroidId;

#elif UNITY_IPHONE
            return IosId;
#else
            return "unused";
#endif
        }

        public int GetRetryInterval()
        {
            if (!RemoteConfigService.IsRemoteConfigFetched)
            {
                return RetryInterval;
            }
            
            if (RemoteConfigService.TryGetValue(nameof(RetryInterval), out int retryInterval))
            {
                return retryInterval;
            }

            return RetryInterval;
        }
    }
}