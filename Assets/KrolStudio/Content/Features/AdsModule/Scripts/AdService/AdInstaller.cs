using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/AdInstaller/" + nameof(AdInstaller),
        fileName = nameof(AdInstaller), order = 0)]
    public class AdInstaller : ScriptableObjectInstaller<AdInstaller>
    {
        [SerializeField] private AdConfig _adConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_adConfig).AsSingle();

#if ADMOB
            Container.Bind<IConsentService>()
                .To<ConsentService>()
                .AsSingle();
#else
            Container.Bind<IConsentService>()
                .To<NullConsentService>()
                .AsSingle();
#endif

            Container.Bind<INoAdsService>()
                .To<NoAdsService>()
                .AsSingle();

            // Provider is selected from the config
            switch (_adConfig.ActiveProvider)
            {
                case MediationProvider.AppLovin:
#if APPLOVIN
                    Container.Bind<IMediationProvider>()
                        .To<AppLovinProvider>()
                        .AsSingle();
#endif
                    break;
                case MediationProvider.LevelPlay:
#if LEVELPLAY
                    Container.Bind<IMediationProvider>()
                        .To<LevelPlayProvider>()
                        .AsSingle();
#endif
                    break;
                case MediationProvider.AdMob:
#if ADMOB
                    Container.Bind<IMediationProvider>()
                        .To<AdMobProvider>()
                        .AsSingle();
#endif
                    break;
            }
#if APPLOVIN || LEVELPLAY || ADMOB
            Container.BindInterfacesAndSelfTo<AdService>().AsSingle();
#endif
        }
    }
}