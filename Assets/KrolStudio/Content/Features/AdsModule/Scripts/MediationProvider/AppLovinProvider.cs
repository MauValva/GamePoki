#if APPLOVIN
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KrolStudio
{
    public class AppLovinProvider : IMediationProvider, IDisposable
    {
        private readonly AdConfig _config;
        private readonly ILogService _logService;

        private AdLoadState _interstitialState = AdLoadState.None;
        private AdLoadState _rewardedState = AdLoadState.None;

        private float _interstitialRetryDelay;

        public AdLoadState InterstitialState => _interstitialState;
        public AdLoadState RewardedState => _rewardedState;

        public bool IsInitialized { get; private set; }
        public bool IsInterstitialReady => MaxSdk.IsInterstitialReady(_config.AppLovinInterstitialId);
        public bool IsRewardedReady => MaxSdk.IsRewardedAdReady(_config.AppLovinRewardedId);

        public AppLovinProvider(AdConfig config, ILogService logService)
        {
            _config = config;
            _logService = logService;
            _interstitialRetryDelay = config.RetryDelayInitial;
        }

        public async UniTask InitializeAsync(IConsentService consent)
        {
            // Consent
            MaxSdk.SetHasUserConsent(consent.IsConsentObtained);

#if UNITY_IOS
            MaxSdk.SetDoNotSell(!consent.IsTrackingAuthorized);
#endif

            var tcs = new UniTaskCompletionSource();
            var timeout = UniTask.Delay(TimeSpan.FromSeconds(_config.InitTimeoutSeconds));

            MaxSdkCallbacks.OnSdkInitializedEvent += OnInitialized;
            MaxSdk.InitializeSdk();

            await UniTask.WhenAny(tcs.Task, timeout);

            // We always unsubscribe — both on success and on timeout.
            MaxSdkCallbacks.OnSdkInitializedEvent -= OnInitialized;

            if (!IsInitialized)
            {
                _logService.LogWarning("[AppLovin] Init timeout.");
                return;
            }

            // Banner
            MaxSdk.CreateBanner(_config.AppLovinBannerId, MaxSdkBase.BannerPosition.BottomCenter);
            MaxSdk.HideBanner(_config.AppLovinBannerId);

            // Interstitial
            _interstitialState = AdLoadState.Loading;

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailed;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHidden;

            MaxSdk.LoadInterstitial(_config.AppLovinInterstitialId);

            // Rewarded
            _rewardedState = AdLoadState.Loading;

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedFailed;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedHidden;

            void OnInitialized(MaxSdkBase.SdkConfiguration _)
            {
                IsInitialized = true;
                _logService.Log("[AppLovin] SDK initialized.");
                tcs.TrySetResult();
            }
        }

        // Banner

        public void ShowBanner() => MaxSdk.ShowBanner(_config.AppLovinBannerId);
        public void HideBanner() => MaxSdk.HideBanner(_config.AppLovinBannerId);

        // Interstitial

        private void OnInterstitialLoaded(string id, MaxSdkBase.AdInfo info)
        {
            _interstitialState = AdLoadState.Ready;
            _interstitialRetryDelay = _config.RetryDelayInitial; // reset backoff
        }

        private void OnInterstitialHidden(string id, MaxSdkBase.AdInfo info)
        {
            _interstitialState = AdLoadState.Loading;
            MaxSdk.LoadInterstitial(_config.AppLovinInterstitialId);
        }

        private void OnInterstitialLoadFailed(string id, MaxSdkBase.ErrorInfo error)
            => OnInterstitialLoadFailedAsync(error).Forget();

        private async UniTaskVoid OnInterstitialLoadFailedAsync(MaxSdkBase.ErrorInfo error)
        {
            _interstitialState = AdLoadState.Failed;
            _logService.LogWarning(
                $"[AppLovin] Interstitial load failed: {error.Message}. " +
                $"Retry in {_interstitialRetryDelay}s.");

            await UniTask.Delay(TimeSpan.FromSeconds(_interstitialRetryDelay));
            _interstitialRetryDelay = Mathf.Min(
                _interstitialRetryDelay * 2f, _config.RetryDelayMax);

            _interstitialState = AdLoadState.Loading;
            MaxSdk.LoadInterstitial(_config.AppLovinInterstitialId);
        }

        public async UniTask<AdResult> ShowInterstitialAsync()
        {
            if (!IsInterstitialReady)
            {
                _logService.LogWarning("[AppLovin] Interstitial not ready.");
                return AdResult.Failed;
            }

            _interstitialState = AdLoadState.None;

            var tcs = new UniTaskCompletionSource<AdResult>();

            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnHidden;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnFailed;

            MaxSdk.ShowInterstitial(_config.AppLovinInterstitialId);

            var timeout = UniTask.Delay(TimeSpan.FromSeconds(10));

            var (which, _) = await UniTask.WhenAny(tcs.Task, timeout);

            if (which)
            {
                Cleanup();
                _logService.LogWarning("[AppLovin] Interstitial show timeout.");
                tcs.TrySetResult(AdResult.Failed);
                return AdResult.Failed;
            }

            return await tcs.Task;

            void OnHidden(string id, MaxSdkBase.AdInfo info)
            {
                Cleanup();
                tcs.TrySetResult(AdResult.Success);
            }

            void OnFailed(string id, MaxSdkBase.ErrorInfo error, MaxSdkBase.AdInfo info)
            {
                Cleanup();
                _logService.LogWarning($"[AppLovin] Interstitial show failed: {error.Message}");
                tcs.TrySetResult(AdResult.Failed);
            }

            void Cleanup()
            {
                MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnHidden;
                MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnFailed;
            }
        }

        // Rewarded

        private void OnRewardedLoaded(string id, MaxSdkBase.AdInfo info)
            => _rewardedState = AdLoadState.Ready;

        private void OnRewardedFailed(string id, MaxSdkBase.ErrorInfo error)
            => _rewardedState = AdLoadState.Failed;

        private void OnRewardedHidden(string id, MaxSdkBase.AdInfo info)
        {
            _rewardedState = AdLoadState.Loading;
            MaxSdk.LoadRewardedAd(_config.AppLovinRewardedId);
        }

        public async UniTask<AdResult> ShowRewardedAsync()
        {
            if (!IsRewardedReady)
            {
                _logService.LogWarning("[AppLovin] Rewarded not ready.");
                return AdResult.Failed;
            }

            var tcs = new UniTaskCompletionSource<AdResult>();
            bool rewarded = false;
            bool closed = false;

            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewarded;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnHidden;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnFailed;

            MaxSdk.ShowRewardedAd(_config.AppLovinRewardedId);

            var timeout = UniTask.Delay(TimeSpan.FromSeconds(10));
            var (which, _) = await UniTask.WhenAny(tcs.Task, timeout);

            if (which)
            {
                Cleanup();
                _logService.LogWarning("[AppLovin] Rewarded show timeout.");
                tcs.TrySetResult(AdResult.Failed);
                return AdResult.Failed;
            }

            return await tcs.Task;

            void OnRewarded(string id, MaxSdkBase.Reward _, MaxSdkBase.AdInfo info)
            {
                rewarded = true;
                if (closed) { Cleanup(); tcs.TrySetResult(AdResult.Success); }
            }

            void OnHidden(string id, MaxSdkBase.AdInfo info)
            {
                closed = true;
                if (rewarded) { Cleanup(); tcs.TrySetResult(AdResult.Success); }
                else WaitAndResolve().Forget();
            }

            async UniTaskVoid WaitAndResolve()
            {
                await UniTask.NextFrame();
                Cleanup();
                tcs.TrySetResult(rewarded ? AdResult.Success : AdResult.Skipped);
            }

            void OnFailed(string id, MaxSdkBase.ErrorInfo error, MaxSdkBase.AdInfo info)
            {
                Cleanup();
                _logService.LogWarning($"[AppLovin] Rewarded show failed: {error.Message}");
                tcs.TrySetResult(AdResult.Failed);
            }

            void Cleanup()
            {
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewarded;
                MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnHidden;
                MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnFailed;
            }
        }

        public void Dispose()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialLoadFailed;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHidden;

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedFailed;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedHidden;
        }
    }
}
#endif