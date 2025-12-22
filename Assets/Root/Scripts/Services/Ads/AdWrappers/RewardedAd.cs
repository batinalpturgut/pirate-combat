using GoogleMobileAds.Api;
using Root.Scripts.Services.Ads.Abstractions;
using Root.Scripts.Services.Ads.ScriptableObjects;
using Root.Scripts.Services.Core.Enums;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Logger.Enums;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;
using RewardedView = GoogleMobileAds.Api.RewardedAd;

namespace Root.Scripts.Services.Ads.AdWrappers
{
    public class RewardedAd : IAd
    {
        private RewardedAdConfig _adConfig;
        private RewardedView _rewardedView;
        private MonoBehaviour _coroutineRunner;

        public AdState AdState { get; private set; } = AdState.Pending;
        public bool CanShowAd => AdState == AdState.Complete;

        public RewardedAd(MonoBehaviour coroutineRunner, RewardedAdConfig adConfig)
        {
            _coroutineRunner = coroutineRunner;
            _adConfig = adConfig;
        }
        
        public void LoadAd()
        {
        }

        public void ShowAd()
        {
        }

        public void HideAd()
        {
        }

        private void DestroyAd()
        {
            if (_rewardedView == null)
            {
                return;
            }
            
            UnsubscribeEventHandlers();
            _rewardedView.Destroy();
            _rewardedView = null;
            AdState = AdState.Pending;
        }

        private void SubscribeEventHandlers()
        {
            // Raised when the ad is estimated to have earned money.
            _rewardedView.OnAdPaid += AdPaid;

            // Raised when an impression is recorded for an ad.
            _rewardedView.OnAdImpressionRecorded += AdImpressionRecorded;

            // Raised when a click is recorded for an ad.
            _rewardedView.OnAdClicked += AdClicked;

            // Raised when an ad opened full screen content.
            _rewardedView.OnAdFullScreenContentOpened += AdFullScreenContentOpened;

            // Raised when the ad closed full screen content.
            _rewardedView.OnAdFullScreenContentClosed += AdFullScreenContentClosed;

            // Raised when the ad failed to open full screen content.
            _rewardedView.OnAdFullScreenContentFailed += AdFullScreenContentFailed;
        }

        private void UnsubscribeEventHandlers()
        {
            _rewardedView.OnAdPaid -= AdPaid;
            _rewardedView.OnAdImpressionRecorded -= AdImpressionRecorded;
            _rewardedView.OnAdClicked -= AdClicked;
            _rewardedView.OnAdFullScreenContentOpened -= AdFullScreenContentOpened;
            _rewardedView.OnAdFullScreenContentClosed -= AdFullScreenContentClosed;
            _rewardedView.OnAdFullScreenContentFailed -= AdFullScreenContentFailed;
        }

        private void AdPaid(AdValue adValue)
        {
            Log.Console($"Rewarded ad paid {adValue.Value} {adValue.CurrencyCode}.", LogType.Debug,
                LogContext.Always, Prefix.GameServices);
        }

        private void AdImpressionRecorded()
        {
            Log.Console("Rewarded ad recorded an impression.", LogType.Debug, LogContext.Always,
                Prefix.GameServices);
        }

        private void AdClicked()
        {
            Log.Console("Rewarded ad was clicked.", LogType.Debug, LogContext.Always, Prefix.GameServices);
        }

        private void AdFullScreenContentOpened()
        {
            Log.Console("Rewarded ad full screen content opened.", LogType.Debug, LogContext.Always,
                Prefix.GameServices);
        }

        private void AdFullScreenContentClosed()
        {
            Log.Console("Rewarded ad full screen content closed.", LogType.Debug, LogContext.Always,
                Prefix.GameServices);
        }

        private void AdFullScreenContentFailed(AdError error)
        {
            Log.Console($"Rewarded ad failed to open full screen content with error: {error}", LogType.Error,
                LogContext.Always, Prefix.GameServices);
        }
    }
}