using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class GameplayInstaller : Installer<GameplayInstaller>
    {
        public override void InstallBindings()
        {
            var assetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            Container.Bind<DefaultProgressConfig>()
              .FromScriptableObject(assetLoaderService.LoadAsset<DefaultProgressConfig>(Address.Configurations.DefaultProgressConfig))
              .AsSingle();

            Container.Bind<ProjectileConfig>()
                .FromScriptableObject(assetLoaderService.LoadAsset<ProjectileConfig>(Address.Configurations.ProjectileConfig))
                .AsSingle();

            Container.Bind<PlayerConfig>()
                .FromScriptableObject(assetLoaderService.LoadAsset<PlayerConfig>(Address.Configurations.PlayerConfig))
                .AsSingle();

            Container.Bind<LevelDatabase>()
               .FromScriptableObject(assetLoaderService.LoadAsset<LevelDatabase>(Address.Configurations.LevelDatabase))
               .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerTransformModel>().AsSingle();
            Container.Bind<PlayerChaseTargetModel>().AsSingle();
            Container.Bind<PlayerDamageableModel>().AsSingle();
            Container.Bind<PlayerEffectsModel>().AsSingle();

            Container.Bind<MovementServiceModel>().AsSingle();
            Container.Bind<DisplayablesModel>().AsSingle();
            Container.Bind<RocketLaunchModel>().AsSingle();
            Container.Bind<BoardServiceModel>().AsSingle();

            Container.Bind<SlideMoverContext>().AsSingle();

            Container.BindInterfacesAndSelfTo<ProgressService>().AsSingle();
        }
    }
}