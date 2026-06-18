using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class UIModuleInstaller : Installer<UIModuleInstaller>
    {
        public override void InstallBindings()
        {
            BindMovementTracker();
            BindRocketLauncher();
            BindUI();
            BindHUD();
            BindUIPresenters();
            BindStatsCalculator();
            BindReward();
        }

        private void BindUI()
        {
            Container.Bind<UIScreen>()
               .FromComponentsInHierarchy()
               .AsCached();

            Container.Bind<TutorialUI>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIScreenNavigationService>().AsSingle();
        }

        private void BindUIPresenters()
        {
            Container.BindInterfacesAndSelfTo<TapToStartPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<CompletePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<FailScreenPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<UpgradeScreenPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<SettingsPresenter>().AsSingle();
        }

        private void BindHUD()
        {
            Container.Bind<HUDView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<HUDPresenter>().AsSingle();
        }

        private void BindMovementTracker()
        {
            Container.BindInterfacesAndSelfTo<MeterProgress>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<MeterProgressPresenter>().AsSingle();
            Container.Bind<MovementTracker>().AsSingle();
        }

        private void BindReward()
        {
            // Config
            var _addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            Container.Bind<RewardConfig>()
                .FromScriptableObject(_addressablesAssetLoaderService.LoadAsset<RewardConfig>(Address.Configurations.RewardConfig))
                .AsSingle();
        }

        private void BindRocketLauncher()
        {
            // Config
            var _addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            Container.Bind<RocketConfig>()
                .FromScriptableObject(_addressablesAssetLoaderService.LoadAsset<RocketConfig>(Address.Configurations.RocketConfig))
                .AsSingle();

            Container.BindInterfacesAndSelfTo<RocketLauncherView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<RocketLauncherPresenter>().AsSingle();
        }

        private void BindStatsCalculator()
        {
            Container.Bind<UpgradeStatsCalculator>().AsSingle();
        }
    }
}