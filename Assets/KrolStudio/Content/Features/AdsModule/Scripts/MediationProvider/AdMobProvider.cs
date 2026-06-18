#if ADMOB
using System;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;

namespace KrolStudio
{
    public class AdMobProvider : IMediationProvider
    {
        private readonly AdConfig _config;
        private readonly ILogService _logService;

        private BannerView _banner;
        private InterstitialAd _interstitial;
        private RewardedAd _rewarded;

        private AdLoadState _interstitialState = AdLoadState.None;
        private AdLoadState _rewardedState = AdLoadState.None;

        private float _interstitialRetryDelay;

        public AdLoadState InterstitialState => _interstitialState;
        public AdLoadState RewardedState => _rewardedState;

        public bool IsInitialized { get; private set; }
        public bool IsInterstitialReady => _interstitial != null && _interstitial.CanShowAd();
        public bool IsRewardedReady => _rewarded != null && _rewarded.CanShowAd();

        public AdMobProvider(AdConfig config, ILogService logService)
        {
            _config = config;
            _logService = logService;
            _interstitialRetryDelay = config.RetryDelayInitial;
        }

        public async UniTask InitializeAsync(IConsentService consent)
        {
            var requestConfiguration = new RequestConfiguration
            {
                TagForUnderAgeOfConsent = TagForUnderAgeOfConsent.False
            };

            MobileAds.SetRequestConfiguration(requestConfiguration);

            var tcs = new UniTaskCompletionSource();
            var timeout = UniTask.Delay(TimeSpan.FromSeconds(_config.InitTimeoutSeconds));

            MobileAds.Initialize(_ =>
            {
                IsInitialized = true;
                _logService.Log("[AdMob] SDK initialized.");
                tcs.TrySetResult();
            });

            await UniTask.WhenAny(tcs.Task, timeout);

            if (!IsInitialized)
            {
                _logService.LogWarning("[AdMob] Init timeout.");
                return;
            }

            LoadBanner();
            LoadInterstitial();
            LoadRewarded();
        }

        // Banner

        public void ShowBanner()
        {
            if (_banner == null) LoadBanner();
            _banner?.Show();
        }

        public void HideBanner() =>
            _banner?.Hide();

        private void LoadBanner()
        {
            _banner?.Destroy();
            _banner = new BannerView(_config.AdMobBannerId, AdSize.Banner, AdPosition.Bottom);
            _banner.LoadAd(new AdRequest());
            _banner.Hide();
        }

        // Interstitial

        public async UniTask<AdResult> ShowInterstitialAsync()
        {
            if (!IsInterstitialReady)
            {
                _logService.LogWarning("[AdMob] Interstitial not ready.");
                return AdResult.Failed;
            }

            _interstitialState = AdLoadState.None;

            var tcs = new UniTaskCompletionSource<AdResult>();
            var ad = _interstitial;

            ad.OnAdFullScreenContentClosed += OnClosed;
            ad.OnAdFullScreenContentFailed += OnFailed;

            ad.Show();

            var timeout = UniTask.Delay(TimeSpan.FromSeconds(10));
            var (which, _) = await UniTask.WhenAny(tcs.Task, timeout);

            if (which)
            {
                Cleanup();
                _logService.LogWarning("[AdMob] Interstitial show timeout.");
                _interstitialState = AdLoadState.Loading;
                LoadInterstitial();
                tcs.TrySetResult(AdResult.Failed);
                return AdResult.Failed;
            }

            return await tcs.Task;

            void OnClosed()
            {
                _interstitialState = AdLoadState.Loading;
                Cleanup();
                LoadInterstitial();
                tcs.TrySetResult(AdResult.Success);
            }

            void OnFailed(AdError error)
            {
                Cleanup();
                _logService.LogWarning($"[AdMob] Interstitial show failed: {error.GetMessage()}");
                tcs.TrySetResult(AdResult.Failed);
            }

            void Cleanup()
            {
                ad.OnAdFullScreenContentClosed -= OnClosed;
                ad.OnAdFullScreenContentFailed -= OnFailed;
            }
        }

        private void LoadInterstitial()
        {
            _interstitialState = AdLoadState.Loading;

            InterstitialAd.Load(_config.AdMobInterstitialId, new AdRequest(),
                (ad, error) =>
                {
                    if (error != null)
                    {
                        _interstitialState = AdLoadState.Failed;
                        _logService.LogWarning(
                            $"[AdMob] Interstitial load failed: {error.GetMessage()}. " +
                            $"Retry in {_interstitialRetryDelay}s.");
                        RetryInterstitialAsync().Forget();
                        return;
                    }

                    _interstitial = ad;
                    _interstitialState = AdLoadState.Ready;
                    _interstitialRetryDelay = _config.RetryDelayInitial;
                    _logService.Log("[AdMob] Interstitial loaded.");
                });
        }

        private async UniTaskVoid RetryInterstitialAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_interstitialRetryDelay));
            _interstitialRetryDelay = Mathf.Min(
                _interstitialRetryDelay * 2f, _config.RetryDelayMax);
            LoadInterstitial();
        }

        // Rewarded

        public async UniTask<AdResult> ShowRewardedAsync()
        {
            if (!IsRewardedReady)
            {
                _logService.LogWarning("[AdMob] Rewarded not ready.");
                return AdResult.Failed;
            }

            var tcs = new UniTaskCompletionSource<AdResult>();
            bool rewarded = false;

            _rewarded.OnAdFullScreenContentClosed += OnClosed;
            _rewarded.OnAdFullScreenContentFailed += OnFailed;

            _rewarded.Show(_ => { rewarded = true; });

            var timeout = UniTask.Delay(TimeSpan.FromSeconds(10));
            var (which, _) = await UniTask.WhenAny(tcs.Task, timeout);

            if (which)
            {
                Cleanup();
                _logService.LogWarning("[AdMob] Rewarded show timeout.");
                tcs.TrySetResult(AdResult.Failed);
                return AdResult.Failed;
            }

            return await tcs.Task;

            void OnClosed()
            {
                Cleanup();
                LoadRewarded();
                WaitAndResolve().Forget();
            }

            async UniTaskVoid WaitAndResolve()
            {
                await UniTask.NextFrame();
                tcs.TrySetResult(rewarded ? AdResult.Success : AdResult.Skipped);
            }

            void OnFailed(AdError error)
            {
                Cleanup();
                _logService.LogWarning($"[AdMob] Rewarded show failed: {error.GetMessage()}");
                tcs.TrySetResult(AdResult.Failed);
            }

            void Cleanup()
            {
                _rewarded.OnAdFullScreenContentClosed -= OnClosed;
                _rewarded.OnAdFullScreenContentFailed -= OnFailed;
            }
        }

        private void LoadRewarded()
        {
            _rewardedState = AdLoadState.Loading;

            RewardedAd.Load(_config.AdMobRewardedId, new AdRequest(),
                (ad, error) =>
                {
                    if (error != null)
                    {
                        _rewardedState = AdLoadState.Failed;
                        _logService.LogWarning($"[AdMob] Rewarded load failed: {error.GetMessage()}");
                        return;
                    }

                    _rewarded = ad;
                    _rewardedState = AdLoadState.Ready;
                    _logService.Log("[AdMob] Rewarded loaded.");
                });
        }
    }
}
#endif