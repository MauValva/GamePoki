using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public interface IAdService
    {
        bool IsInitialized { get; }
        bool IsNoAds { get; }

        UniTask InitializeAsync();

        void ShowBanner();
        void HideBanner();

        UniTask<AdResult> ShowInterstitialAsync(int currentLevel);
        UniTask<AdResult> ShowRewardedAsync();
    }
}