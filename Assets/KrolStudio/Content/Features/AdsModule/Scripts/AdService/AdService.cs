using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    /*
    UI
    if (_adService.InterstitialState == AdLoadState.Loading)
        ShowLoadingSpinner();

    if (_adService.InterstitialState == AdLoadState.Failed)
        ShowNoAdsMessage(); 
     */

    public class AdService : IAdService, IInitializable, IDisposable
    {
        private readonly IMediationProvider _provider;
        private readonly IConsentService _consent;
        private readonly INoAdsService _noAds;
        private readonly AdConfig _config;
        private readonly ILogService _logService;
        private readonly SignalBus _signalBus;

        private float _lastInterstitialTime = float.NegativeInfinity;
        private bool _isInitializing;

        public bool IsInitialized => _provider.IsInitialized;
        public bool IsNoAds => _noAds.IsPurchased;

        [Inject]
        public AdService(
            IMediationProvider provider,
            IConsentService consent,
            INoAdsService noAds,
            AdConfig config,
            ILogService logService,
            SignalBus signalBus)
        {
            _provider = provider;
            _consent = consent;
            _noAds = noAds;
            _config = config;
            _logService = logService;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            InitializeAsync().Forget();
            _signalBus.Subscribe<GameplayStartedSignal>(ShowBanner);
        }

        public void Dispose() 
        { 
            _signalBus.Unsubscribe<GameplayStartedSignal>(ShowBanner);
        }

        public async UniTask InitializeAsync()
        {
            if (IsInitialized || _isInitializing) return;
            _isInitializing = true;

            try
            {
                // 1. Consent (Google UMP)
                await _consent.RequestConsentAsync();

                // 2. IDFA (iOS)
                await _consent.RequestTrackingAsync();

                // 3. Init provider
                await _provider.InitializeAsync(_consent);

                if (!IsNoAds)
                    _provider.ShowBanner();
            }
            catch (Exception ex)
            {
                _logService.LogError($"[AdService] Init exception: {ex.Message}");
            }
            finally
            {
                _isInitializing = false;
            }
        }

        public void ShowBanner()
        {
            if (!IsInitialized || IsNoAds) return;
            _provider.ShowBanner();
        }

        public void HideBanner()
        {
            if (!IsInitialized) return;
            _provider.HideBanner();
        }

        public async UniTask<AdResult> ShowInterstitialAsync(int currentLevel)
        {
            if (!IsInitialized) return AdResult.Failed;
            if (IsNoAds) return AdResult.NoAds;
            if (currentLevel < _config.MinLevelToShow) return AdResult.Skipped;

            if (_provider.InterstitialState == AdLoadState.Loading)
            {
                _logService.Log("[Ad] Interstitial still loading.");
                return AdResult.Skipped;
            }

            if (_provider.InterstitialState == AdLoadState.Failed)
            {
                _logService.LogWarning("[Ad] Interstitial failed to load.");
                return AdResult.Failed;
            }

            if (_provider.InterstitialState != AdLoadState.Ready)
                return AdResult.Failed;

            float elapsed = Time.time - _lastInterstitialTime;
            if (elapsed < _config.CooldownSeconds)
            {
                _logService.Log(
                    $"[Ad] Interstitial cooldown: {_config.CooldownSeconds - elapsed:F0}s left.");
                return AdResult.Skipped;
            }

            var result = await _provider.ShowInterstitialAsync();

            if (result == AdResult.Success)
                _lastInterstitialTime = Time.time;

            return result;
        }

        public async UniTask<AdResult> ShowRewardedAsync()
        {
            if (!IsInitialized) return AdResult.Failed;

            if (IsNoAds && _config.BlockRewardedOnNoAds) return AdResult.NoAds;

            if (!_provider.IsRewardedReady) return AdResult.Failed;

            return await _provider.ShowRewardedAsync();
        }
    }
}