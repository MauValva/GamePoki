using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public interface IMediationProvider
    {
        bool IsInitialized { get; }
        bool IsInterstitialReady { get; }
        bool IsRewardedReady { get; }

        AdLoadState InterstitialState { get; }
        AdLoadState RewardedState { get; }

        UniTask InitializeAsync(IConsentService consent);

        void ShowBanner();
        void HideBanner();
        UniTask<AdResult> ShowInterstitialAsync();
        UniTask<AdResult> ShowRewardedAsync();
    }
}