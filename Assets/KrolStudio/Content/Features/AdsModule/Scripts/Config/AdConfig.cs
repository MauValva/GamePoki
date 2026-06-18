using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/AdConfig", fileName = nameof(AdConfig))]
    public class AdConfig : ScriptableObject
    {
        [Header("Active Provider")]
        public MediationProvider ActiveProvider = MediationProvider.LevelPlay;

        [Header("AppLovin MAX")]
        public string AppLovinBannerId;
        public string AppLovinInterstitialId;
        public string AppLovinRewardedId;

        [Header("LevelPlay (IronSource)")]
        public string LevelPlayAppKey;
        [Tooltip("For classic IronSource mediation — leave empty.")]
        public string LevelPlayBannerId;
        public string LevelPlayInterstitialId;
        public string LevelPlayRewardedId;

        [Header("AdMob (Direct)")]
        public string AdMobBannerId;
        public string AdMobInterstitialId;
        public string AdMobRewardedId;

        [Header("Interstitial Rules")]
        [Min(0)] public int MinLevelToShow = 3;
        [Min(0)] public int CooldownSeconds = 30;

        [Header("Init / Retry")]
        [Min(5f)] public float InitTimeoutSeconds = 10f;
        [Min(1f)] public float RetryDelayInitial = 5f;
        [Min(10f)] public float RetryDelayMax = 60f;

        [Header("Rewarded")]
        [Tooltip("Block Rewarded after purchasing No Ads?")]
        public bool BlockRewardedOnNoAds = false;
    }
}