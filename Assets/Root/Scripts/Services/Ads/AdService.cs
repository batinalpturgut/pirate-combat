using System;
using GoogleMobileAds.Api;
using NaughtyAttributes;
using Root.Scripts.Extensions;
using Root.Scripts.Services.Ads.Abstractions;
using Root.Scripts.Services.Ads.AdWrappers;
using Root.Scripts.Services.Ads.ScriptableObjects;
using Root.Scripts.Services.Core.Abstractions;
using Root.Scripts.Services.RemoteConfig.Abstractions;
using UnityEngine;
using InterstitialAd = Root.Scripts.Services.Ads.AdWrappers.InterstitialAd;
using RewardedAd = Root.Scripts.Services.Ads.AdWrappers.RewardedAd;

namespace Root.Scripts.Services.Ads
{
    [CreateAssetMenu(fileName = "AdService", menuName = "Services/Ad/AdService", order = 0)]
    public class AdService : AService, IAdService
    {
        [SerializeField, BoxGroup("Configs")] 
        private BannerAdConfig bannerAdConfig;
        
        [SerializeField, BoxGroup("Configs")] 
        private InterstitialAdConfig interstitialAdConfig;
        
        [SerializeField, BoxGroup("Configs")] 
        private RewardedAdConfig rewardedAdConfig;
        
        private IRemoteConfigService _remoteConfigService;
        
        private IAd _bannerAd;
        private IAd _interstitialAd;
        private IAd _rewardedAd;
        private bool _isStarted;
        
        public event Action<bool> RewardedAdWatched;
        public bool CanShowRewardedAd { get; }
        public bool CanShowBannerAd  => _interstitialAd.CanShowAd;

        protected override void OnInitialize()
        {
            _remoteConfigService = GameServicesContext.Resolve<IRemoteConfigService>();
            
            MobileAds.Initialize(_ =>
            {
                _bannerAd = new BannerAd(GameServicesContext, bannerAdConfig);
                _interstitialAd = new InterstitialAd(GameServicesContext, interstitialAdConfig);
                _rewardedAd = new RewardedAd(GameServicesContext, rewardedAdConfig);
                IsInitialized = true;
            });
        }

        protected override void OnStart()
        {
            _remoteConfigService.OnRemoteConfigFetched += () =>
            {

                bannerAdConfig.Initialize(_remoteConfigService);
                interstitialAdConfig.Initialize(_remoteConfigService);
                // rewardedAdConfig.Initialize(_remoteConfigService);

                _bannerAd.LoadAd();
                _interstitialAd.LoadAd();
                // _rewardedAd.LoadAd();

                _isStarted = true;
            };
        }

        public void ShowInterstitial()
        {
            GameServicesContext.WaitUntil(this, self => self._isStarted,
                self =>
                {
                    self._interstitialAd.ShowAd();
                });
        }

        public void ShowRewarded()
        {
            GameServicesContext.WaitUntil(this, self => self._isStarted,
                self =>
                {
                    self._rewardedAd.ShowAd();
                });
        }

        public void ShowBanner()
        {
            GameServicesContext.WaitUntil(this, self => self._isStarted,
                self =>
                {
                    self._bannerAd.ShowAd();
                });
        }

        public void HideBanner()
        {
            GameServicesContext.WaitUntil(this, self => self._isStarted,
                self =>
                {
                    self._bannerAd.HideAd();
                });
        }
    }
}