using GoogleMobileAds.Api;
using UnityEngine;

namespace Root.Scripts.Services.Ads.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BannerAdConfig", menuName = "Services/Ad/AdConfig/Banner", order = 0)]
    public class BannerAdConfig : AdConfig
    {
        [field: SerializeField] 
        public AdPosition BannerPosition { get; private set; } = AdPosition.Bottom;
    }
}