using System;
using Root.Scripts.Services.Core.Attributes;

namespace Root.Scripts.Services.Ads.Abstractions
{
    [ServiceAPI]
    public interface IAdService
    {
        event Action<bool> RewardedAdWatched;
        bool CanShowRewardedAd { get; }
        bool CanShowBannerAd { get; }
        void ShowInterstitial();
        void ShowRewarded();
        void ShowBanner();
        void HideBanner();
    }
}