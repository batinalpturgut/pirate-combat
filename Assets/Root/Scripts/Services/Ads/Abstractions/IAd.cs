using Root.Scripts.Services.Core.Enums;

namespace Root.Scripts.Services.Ads.Abstractions
{
    public interface IAd
    {
        AdState AdState { get; }
        bool CanShowAd { get; }
        void LoadAd();
        void ShowAd();
        void HideAd();
    }
}