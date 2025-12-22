using System.Collections;
using GoogleMobileAds.Api;
using Root.Scripts.Services.Ads.Abstractions;
using Root.Scripts.Services.Ads.ScriptableObjects;
using Root.Scripts.Services.Core.Enums;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Guards;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Logger.Enums;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Services.Ads.AdWrappers
{
    public class BannerAd : IAd
    {
        private readonly BannerAdConfig _adConfig;
        private BannerView _bannerView;
        private readonly MonoBehaviour _coroutineRunner;
        private Coroutine _loadAdCoroutine;

        public AdState AdState { get; private set; } = AdState.Pending;
        public bool CanShowAd => AdState == AdState.Complete && 
                                 _bannerView != null;

        public BannerAd(MonoBehaviour coroutineRunner, BannerAdConfig adConfig)
        {
            _coroutineRunner = coroutineRunner;
            _adConfig = adConfig;
        }

        public void LoadAd()
        {
            Guard.Against.Null(_adConfig, nameof(AdConfig));
            
            DestroyAd();
            Log.Console("Creating banner view", LogType.Debug, LogContext.Always, Prefix.GameServices);

            AdState = AdState.InProgress;
            _bannerView = new BannerView(_adConfig.GetId(), AdSize.Banner, _adConfig.BannerPosition);
            _bannerView.Hide();

            SubscribeEventHandlers();
        }

        public void ShowAd()
        {
            _bannerView.Show();
        }

        public void HideAd()
        {
            _bannerView.Hide();
        }

        private void DestroyAd()
        {
            if (_bannerView == null)
            {
                return;
            }

            Log.Console("Destroying banner view.", LogType.Debug, LogContext.Always, Prefix.GameServices);
            UnsubscribeEventHandlers();
            _bannerView.Destroy();
            _bannerView = null;
            AdState = AdState.Pending;
        }
        
        private void SubscribeEventHandlers()
        {
            // TODO: Callback'lerin hicbirisi calismiyor!!!
            
            // Raised when an ad is loaded into the banner view.
            _bannerView.OnBannerAdLoaded += BannerAdLoaded;

            // Raised when an ad fails to load into the banner view.
            _bannerView.OnBannerAdLoadFailed += BannerAdLoadFailed;

            // Raised when the ad is estimated to have earned money.
            _bannerView.OnAdPaid += AdPaid;

            // Raised when an impression is recorded for an ad.
            _bannerView.OnAdImpressionRecorded += AdImpressionRecorded;

            // Raised when a click is recorded for an ad.
            _bannerView.OnAdClicked += AdClicked;

            // Raised when an ad opened full screen content.
            _bannerView.OnAdFullScreenContentOpened += AdFullScreenContentOpened;

            // Raised when the ad closed full screen content.
            _bannerView.OnAdFullScreenContentClosed += AdFullScreenContentClosed;
        }

        private void UnsubscribeEventHandlers()
        {
            _bannerView.OnBannerAdLoaded -= BannerAdLoaded;
            _bannerView.OnBannerAdLoadFailed -= BannerAdLoadFailed;
            _bannerView.OnAdPaid -= AdPaid;
            _bannerView.OnAdImpressionRecorded -= AdImpressionRecorded;
            _bannerView.OnAdClicked -= AdClicked;
            _bannerView.OnAdFullScreenContentOpened -= AdFullScreenContentOpened;
            _bannerView.OnAdFullScreenContentClosed -= AdFullScreenContentClosed;
        }

        private void BannerAdLoaded()
        {
            AdState = AdState.Complete;
            if (_loadAdCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_loadAdCoroutine);
                _loadAdCoroutine = null;
            }
        }

        private void BannerAdLoadFailed(LoadAdError error)
        {
            AdState = AdState.InProgress;
            if (_loadAdCoroutine == null)
            {
                _loadAdCoroutine = _coroutineRunner.StartCoroutine(TryLoadAd());
            }
        }

        private IEnumerator TryLoadAd()
        {
            while (AdState != AdState.Complete)
            {
                yield return Wait.ForSecondsRealtime(_adConfig.GetRetryInterval());
                LoadAd();
            }

            _loadAdCoroutine = null;
        }

        private void AdPaid(AdValue adValue)
        {
            Log.Console($"Banner view paid {adValue.Value} {adValue.CurrencyCode}", LogType.Debug, LogContext.Always,
                Prefix.GameServices);
        }

        private void AdImpressionRecorded()
        {
            Log.Console("Banner view recorded an impression.", LogType.Debug, LogContext.Always, Prefix.GameServices);
        }

        private void AdClicked()
        {
            Log.Console("Banner view was clicked.", LogType.Debug, LogContext.Always, Prefix.GameServices);
        }

        private void AdFullScreenContentOpened()
        {
            Log.Console("Banner view full screen content opened.", LogType.Debug, LogContext.Always,
                Prefix.GameServices);
        }

        private void AdFullScreenContentClosed()
        {
            Log.Console("Banner view full screen content closed.", LogType.Debug, LogContext.Always,
                Prefix.GameServices);
        }
    }
}