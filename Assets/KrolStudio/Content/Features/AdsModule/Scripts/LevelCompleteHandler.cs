using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class LevelCompleteHandler : IInitializable
    {
        private readonly IAdService _adService;
        private readonly LevelModel _levelModel;
        private readonly ILogService _logService;

        [Inject]
        public LevelCompleteHandler(
            IAdService adService,
            LevelModel levelModel,
            ILogService logService)
        {
            _adService = adService;
            _levelModel = levelModel;
            _logService = logService;
        }

        public void Initialize() { }

        public async UniTask OnLevelComplete()
        {
            var result = await _adService.ShowInterstitialAsync(_levelModel.CurrentIndex);

            switch (result)
            {
                case AdResult.Success:
                    _logService.Log("[Ad] Interstitial shown.");
                    break;
                case AdResult.NoAds:
                    _logService.Log("[Ad] No Ads — skipped.");
                    break;
                case AdResult.Skipped:
                    _logService.Log("[Ad] Interstitial skipped.");
                    break;
                case AdResult.Failed:
                    _logService.LogWarning("[Ad] Interstitial not ready.");
                    break;
            }
        }

        public async UniTask OnWatchAdClicked(Action onSuccess)
        {
            var result = await _adService.ShowRewardedAsync();
            if (result == AdResult.Success)
                onSuccess?.Invoke();
        }

        public void OnNoAdsPurchased()
        {
            _adService.HideBanner();
            _logService.Log("[Ad] Banner hidden after No Ads purchase.");
        }
    }
}