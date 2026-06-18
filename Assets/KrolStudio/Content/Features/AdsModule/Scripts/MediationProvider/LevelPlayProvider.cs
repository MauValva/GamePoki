#if LEVELPLAY
using System;
using Cysharp.Threading.Tasks;
using Unity.Services.LevelPlay;
using UnityEngine;

namespace KrolStudio
{
    public class LevelPlayProvider : IMediationProvider, IDisposable
    {
        private readonly AdConfig _config;
        private readonly ILogService _logService;

        private LevelPlayInterstitialAd _interstitial;
        private LevelPlayRewardedAd _rewarded;
        private LevelPlayBannerAd _banner;

        private Action<LevelPlayAdError> _onInterstitialLoadFailed;
        private Action<LevelPlayAdError> _onRewardedLoadFailed;

        private float _interstitialRetryDelay;

        private int _attempts = 0;
        private const int _maxAttempts = 5;

        private AdLoadState _interstitialState = AdLoadState.None;
        private AdLoadState _rewardedState = AdLoadState.None;

        public AdLoadState InterstitialState => _interstitialState;
        public AdLoadState RewardedState => _rewardedState;

        public bool IsInitialized { get; private set; }
        public bool IsInterstitialReady => _interstitial?.IsAdReady() ?? false;
        public bool IsRewardedReady => _rewarded?.IsAdReady() ?? false;

        public LevelPlayProvider(AdConfig config, ILogService logService)
        {
            _config = config;
            _logService = logService;
            _interstitialRetryDelay = config.RetryDelayInitial;
        }

        public async UniTask InitializeAsync(IConsentService consent)
        {
            LevelPlay.SetConsent(consent.IsConsentObtained);

            float retryDelay = _config.RetryDelayInitial;

            while (!IsInitialized && _attempts < _maxAttempts)
            {
                _attempts++;

                var tcs = new UniTaskCompletionSource();
                var timeout = UniTask.Delay(TimeSpan.FromSeconds(_config.InitTimeoutSeconds));

                LevelPlay.OnInitSuccess += OnInitSuccess;
                LevelPlay.OnInitFailed += OnInitFailed;

                LevelPlay.Init(_config.LevelPlayAppKey);

                await UniTask.WhenAny(tcs.Task, timeout);

                LevelPlay.OnInitSuccess -= OnInitSuccess;
                LevelPlay.OnInitFailed -= OnInitFailed;

                if (!IsInitialized)
                {
                    _logService.LogWarning($"[LevelPlay] Init timeout. Retry in {retryDelay}s.");
                    await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
                    retryDelay = Mathf.Min(retryDelay * 2f, _config.RetryDelayMax);
                }

                void OnInitSuccess(LevelPlayConfiguration cfg)
                {
                    IsInitialized = true;
                    _logService.Log("[LevelPlay] SDK initialized.");
                    tcs.TrySetResult();
                }

                void OnInitFailed(LevelPlayInitError error)
                {
                    _logService.LogWarning($"[LevelPlay] Init failed: {error.ErrorMessage}");
                    tcs.TrySetResult();
                }
            }

            if (!IsInitialized) return;

            SetupInterstitial();
            SetupRewarded();
            LoadBanner();
        }

        // Banner

        private void LoadBanner()
        {
            _banner = new LevelPlayBannerAd(_config.LevelPlayBannerId);
            
            _banner.LoadAd();
            _banner.HideAd();
        }

        public void ShowBanner() => _banner?.ShowAd();
        public void HideBanner() => _banner?.HideAd();

        // Interstitial

        private void SetupInterstitial()
        {
            _interstitial = new LevelPlayInterstitialAd(_config.LevelPlayInterstitialId);

            _interstitial.OnAdLoaded += OnInterstitialLoaded;

            _onInterstitialLoadFailed = error => OnInterstitialLoadFailedAsync(error).Forget();
            _interstitial.OnAdLoadFailed += _onInterstitialLoadFailed;

            _interstitialState = AdLoadState.Loading;
            _interstitial.LoadAd();
        }

        private void OnInterstitialLoaded(LevelPlayAdInfo info)
        {
            _interstitialState = AdLoadState.Ready;
            _interstitialRetryDelay = _config.RetryDelayInitial;
        }

        private async UniTaskVoid OnInterstitialLoadFailedAsync(LevelPlayAdError error)
        {
            _interstitialState = AdLoadState.Failed;
            _logService.LogWarning(
                $"[LevelPlay] Interstitial load failed: {error.ErrorMessage}. " +
                $"Retry in {_interstitialRetryDelay}s.");

            await UniTask.Delay(TimeSpan.FromSeconds(_interstitialRetryDelay));
            _interstitialRetryDelay = Mathf.Min(
                _interstitialRetryDelay * 2f, _config.RetryDelayMax);

            _interstitialState = AdLoadState.Loading;
            _interstitial.LoadAd();
        }

        public async UniTask<AdResult> ShowInterstitialAsync()
        {
            if (!IsInterstitialReady)
            {
                _logService.LogWarning("[LevelPlay] Interstitial not ready.");
                return AdResult.Failed;
            }

            _interstitialState = AdLoadState.None;

            var tcs = new UniTaskCompletionSource<AdResult>();

            _interstitial.OnAdDisplayed += OnDisplayed;
            _interstitial.OnAdDisplayFailed += OnDisplayFailed;
            _interstitial.OnAdClosed += OnClosed;

            _interstitial.ShowAd();

            var timeout = UniTask.Delay(TimeSpan.FromSeconds(10));
            var (which, _) = await UniTask.WhenAny(tcs.Task, timeout);

            if (which)
            {
                Cleanup();
                _logService.LogWarning("[LevelPlay] Interstitial show timeout.");
                tcs.TrySetResult(AdResult.Failed);
                return AdResult.Failed;
            }

            return await tcs.Task;

            void OnDisplayed(LevelPlayAdInfo info) { }

            void OnClosed(LevelPlayAdInfo info)
            {
                Cleanup();
                _interstitialState = AdLoadState.Loading;
                _interstitial.LoadAd();
                tcs.TrySetResult(AdResult.Success);
            }

            void OnDisplayFailed(LevelPlayAdInfo info, LevelPlayAdError error)
            {
                Cleanup();
                _logService.LogWarning(
                    $"[LevelPlay] Interstitial show failed: {error.ErrorMessage}");
                tcs.TrySetResult(AdResult.Failed);
            }

            void Cleanup()
            {
                _interstitial.OnAdDisplayed -= OnDisplayed;
                _interstitial.OnAdDisplayFailed -= OnDisplayFailed;
                _interstitial.OnAdClosed -= OnClosed;
            }
        }

        // Rewarded

        private void SetupRewarded()
        {
            _rewarded = new LevelPlayRewardedAd(_config.LevelPlayRewardedId);

            _rewarded.OnAdLoaded += OnRewardedLoaded;

            _onRewardedLoadFailed = _ => { _rewardedState = AdLoadState.Failed; };
            _rewarded.OnAdLoadFailed += _onRewardedLoadFailed;

            _rewardedState = AdLoadState.Loading;
            _rewarded.LoadAd();
        }

        private void OnRewardedLoaded(LevelPlayAdInfo info)
            => _rewardedState = AdLoadState.Ready;

        public async UniTask<AdResult> ShowRewardedAsync()
        {
            if (!IsRewardedReady)
            {
                _logService.LogWarning("[LevelPlay] Rewarded not ready.");
                return AdResult.Failed;
            }

            var tcs = new UniTaskCompletionSource<AdResult>();
            bool rewarded = false;

            _rewarded.OnAdRewarded += OnRewarded;
            _rewarded.OnAdClosed += OnClosed;
            _rewarded.OnAdDisplayFailed += OnDisplayFailed;

            _rewarded.ShowAd();

            var timeout = UniTask.Delay(TimeSpan.FromSeconds(10));
            var (which, _) = await UniTask.WhenAny(tcs.Task, timeout);

            if (which)
            {
                Cleanup();
                _logService.LogWarning("[LevelPlay] Rewarded show timeout.");
                tcs.TrySetResult(AdResult.Failed);
                return AdResult.Failed;
            }

            return await tcs.Task;

            void OnRewarded(LevelPlayAdInfo info, LevelPlayReward reward)
                => rewarded = true;

            void OnClosed(LevelPlayAdInfo info)
            {
                Cleanup();
                _rewarded.LoadAd();
                WaitAndResolve().Forget();
            }

            async UniTaskVoid WaitAndResolve()
            {
                await UniTask.NextFrame();
                tcs.TrySetResult(rewarded ? AdResult.Success : AdResult.Skipped);
            }

            void OnDisplayFailed(LevelPlayAdInfo info, LevelPlayAdError error)
            {
                Cleanup();
                _logService.LogWarning(
                    $"[LevelPlay] Rewarded show failed: {error.ErrorMessage}");
                tcs.TrySetResult(AdResult.Failed);
            }

            void Cleanup()
            {
                _rewarded.OnAdRewarded -= OnRewarded;
                _rewarded.OnAdClosed -= OnClosed;
                _rewarded.OnAdDisplayFailed -= OnDisplayFailed;
            }
        }

        public void Dispose()
        {
            if (_interstitial != null)
            {
                _interstitial.OnAdLoaded -= OnInterstitialLoaded;
                _interstitial.OnAdLoadFailed -= _onInterstitialLoadFailed;
            }

            if (_rewarded != null)
            {
                _rewarded.OnAdLoaded -= OnRewardedLoaded;
                _rewarded.OnAdLoadFailed -= _onRewardedLoadFailed;
            }

            _banner?.DestroyAd();
        }
    }
}
#endif