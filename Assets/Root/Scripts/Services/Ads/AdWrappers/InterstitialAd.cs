using System.Collections;
using GoogleMobileAds.Api;
using Root.Scripts.Services.Ads.Abstractions;
using Root.Scripts.Services.Ads.ScriptableObjects;
using Root.Scripts.Services.Core.Enums;
using Root.Scripts.Utilities;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Logger.Enums;
using UnityEngine;
using InterstitialView = GoogleMobileAds.Api.InterstitialAd;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Services.Ads.AdWrappers
{
    public class InterstitialAd : IAd
    {
        private readonly InterstitialAdConfig _adConfig;
        private InterstitialView _interstitialView;
        private readonly MonoBehaviour _coroutineRunner;
        private Coroutine _loadAdCoroutine;
        private float _lastAdShownTime = Mathf.NegativeInfinity;

        public AdState AdState { get; private set; } = AdState.Pending;
        public bool CanShowAd => AdState == AdState.Complete &&
                                 _interstitialView != null && 
                                 _interstitialView.CanShowAd() &&
                                 Time.realtimeSinceStartup - _lastAdShownTime >= _adConfig.GetIntervalBetweenAds();

        public InterstitialAd(MonoBehaviour coroutineRunner, InterstitialAdConfig adConfig)
        {
            _adConfig = adConfig;
            _coroutineRunner = coroutineRunner;
        }

        public void LoadAd()
        {
            DestroyAd();
            Log.Console("Loading the interstitial ad.", LogType.Debug, LogContext.Always, Prefix.GameServices);

            AdState = AdState.InProgress;

            AdRequest adRequest = new AdRequest();

            InterstitialView.Load(_adConfig.GetId(), adRequest, (ad, error) =>
            {
                if (error != null || ad == null)
                {
                    Log.Console($"Interstitial ad failed to load an ad with error: {error}", LogType.Error,
                        LogContext.Always, Prefix.GameServices);

                    AdState = AdState.Pending;

                    if (_loadAdCoroutine == null)
                    {
                        _coroutineRunner.StartCoroutine(TryLoadAd());
                    }

                    return;
                }

                Log.Console($"Interstitial ad loaded with response: {ad.GetResponseInfo()}", LogType.Debug,
                    LogContext.Always, Prefix.GameServices);

                _interstitialView = ad;
                AdState = AdState.Complete;

                if (_loadAdCoroutine != null)
                {
                    _coroutineRunner.StopCoroutine(_loadAdCoroutine);
                    _loadAdCoroutine = null;
                }

                SubscribeEventHandlers();
            });
        }

        public void ShowAd()
        {
            if (CanShowAd)
            {
                Log.Console("Showing interstitial ad.", LogType.Debug, LogContext.Always,
                    Prefix.GameServices);
                _interstitialView.Show();
                _lastAdShownTime = Time.realtimeSinceStartup;
            }
            else
            {
                Log.Console("Interstitial ad is not ready yet.", LogType.Error, LogContext.Always,
                    Prefix.GameServices);
            }
        }

        public void HideAd()
        {
        }

        private void DestroyAd()
        {
            if (_interstitialView == null)
            {
                return;
            }

            UnsubscribeEventHandlers();
            _interstitialView.Destroy();
            _interstitialView = null;
            AdState = AdState.Pending;
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
        
        private void SubscribeEventHandlers()
        {
            // Raised when the ad is estimated to have earned money.
            _interstitialView.OnAdPaid += AdPaid;

            // Raised when an impression is recorded for an ad.
            _interstitialView.OnAdImpressionRecorded += AdImpressionRecorded;

            // Raised when a click is recorded for an ad.
            _interstitialView.OnAdClicked += AdClicked;

            // Raised when an ad opened full screen content.
            _interstitialView.OnAdFullScreenContentOpened += AdFullScreenContentOpened;

            // Raised when the ad closed full screen content.
            _interstitialView.OnAdFullScreenContentClosed += AdFullScreenContentClosed;

            // Raised when the ad failed to open full screen content.
            _interstitialView.OnAdFullScreenContentFailed += AdFullScreenContentFailed;
        }
        
        private void UnsubscribeEventHandlers()
        {
            _interstitialView.OnAdPaid -= AdPaid;
            _interstitialView.OnAdImpressionRecorded -= AdImpressionRecorded;
            _interstitialView.OnAdClicked -= AdClicked;
            _interstitialView.OnAdFullScreenContentOpened -= AdFullScreenContentOpened;
            _interstitialView.OnAdFullScreenContentClosed -= AdFullScreenContentClosed;
            _interstitialView.OnAdFullScreenContentFailed -= AdFullScreenContentFailed;
        }

        private void AdPaid(AdValue adValue)
        {
            Log.Console($"Interstitial ad paid {adValue.Value} {adValue.CurrencyCode}.", LogType.Debug,
                LogContext.Always, Prefix.GameServices);
        }

        private void AdImpressionRecorded()
        {
            Log.Console("Interstitial ad recorded an impression.", LogType.Debug, LogContext.Always,
                Prefix.GameServices);
        }

        private void AdClicked()
        {
            Log.Console("Interstitial ad was clicked.", LogType.Debug, LogContext.Always, Prefix.GameServices);
        }

        private void AdFullScreenContentOpened()
        {
            Log.Console("Interstitial ad full screen content opened.", LogType.Debug, LogContext.Always,
                Prefix.GameServices);
        }

        private void AdFullScreenContentClosed()
        {
            Log.Console("Interstitial ad full screen content closed.", LogType.Debug, LogContext.Always,
                Prefix.GameServices);
            LoadAd();
        }

        private void AdFullScreenContentFailed(AdError error)
        {
            Log.Console($"Interstitial ad failed to open full screen content with error: {error}", LogType.Error,
                LogContext.Always, Prefix.GameServices);
            LoadAd();
        }
    }
}