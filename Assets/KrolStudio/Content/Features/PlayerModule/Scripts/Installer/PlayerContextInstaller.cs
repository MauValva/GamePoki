using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class PlayerContextInstaller : MonoInstaller<PlayerContextInstaller>
    {
        public override void InstallBindings()
        {      
            PrefabSpawnerInstaller.Install(Container);

            Container.Bind<PlayerStatesFactory>().AsSingle();

            // Config
            var _addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();      
            Container.Bind<TurretConfig>()
                .FromScriptableObject(_addressablesAssetLoaderService.LoadAsset<TurretConfig>(Address.Configurations.TurretConfig))
                .AsSingle();

            Container.Bind<FragmentExplosionConfig>()
                .FromScriptableObject(_addressablesAssetLoaderService.LoadAsset<FragmentExplosionConfig>(Address.Configurations.FragmentExplosionConfig))
                .AsSingle();

            Container.Bind<PartUpgradeLevelConfig>()
                .FromScriptableObject(_addressablesAssetLoaderService.LoadAsset<PartUpgradeLevelConfig>(Address.Configurations.PartUpgradeLevelConfig))
                .AsSingle();

            // Health bar
            Container.Bind<HealthBarView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerHealthBarPresenter>().AsSingle();

            // Ammo
            Container.Bind<AmmoView>().FromComponentsInHierarchy().AsSingle();
            Container.Bind<AmmoModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<AmmoPresenter>().AsSingle();

            // Rocket
            Container.Bind<Rocket>().FromComponentsInHierarchy().AsCached();
            Container.BindInterfacesAndSelfTo<RocketLaunchService>().AsSingle();

            // Stats
            Container.Bind<PlayerHealthStatsModel>().AsSingle();
            Container.Bind<PlayerMovementStatsModel>().AsSingle();
            Container.Bind<TurretStatsModel>().AsSingle();
            Container.Bind<ProjectileStatsModel>().AsSingle();
            Container.Bind<RocketStatsModel>().AsSingle();
            Container.Bind<PlayerStatsCalculator>().AsSingle();

            // Dependencies
            Container.Bind<ILaserRenderer>().To<LaserTrajectoryRenderer>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TurretShooting>().AsSingle();
            Container.Bind<TurretRotator>().AsSingle();
            Container.Bind<IPlayerEffects>().To<PlayerEffects>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IWheelRotator>().To<WheelRotator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerDamageable>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerAudioController>().AsSingle();

            // Upgrade
            Container.BindInterfacesAndSelfTo<PlayerService>().AsSingle();

            Container.BindInterfacesAndSelfTo<BoardService>().AsSingle();
            Container.Bind<BoardView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IBoardInteractionService>().To<BoardInteractionService>().AsSingle();
            Container.Bind<IPlayerInteractionService>().To<PlayerInteractionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GarbageView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UpgradeLevelController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IBoardHighlightService>().To<BoardHighlightService>().AsSingle();
        }
    }
}