using UnityEngine;

namespace Root.Scripts.Services.Ads.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InterstitialAdConfig", menuName = "Services/Ad/AdConfig/Interstitial", order = 0)]
    public class InterstitialAdConfig : AdConfig
    {
        [field: SerializeField]
        private int IntervalBetweenAds { get; set; }

        public int GetIntervalBetweenAds()
        {
            if (!RemoteConfigService.IsRemoteConfigFetched)
            {
                return IntervalBetweenAds;
            }
            
            if (RemoteConfigService.TryGetValue(nameof(IntervalBetweenAds), out int intervalBetweenAds))
            {
                return intervalBetweenAds;
            }
            
            
            return IntervalBetweenAds;
        }
    }
}