using Zenject;

namespace KrolStudio
{
    public class LevelModuleInstaller : Installer<LevelModuleInstaller>
    {
        public override void InstallBindings()
        {
            var assetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();

            Container.BindInterfacesAndSelfTo<LevelModel>().AsSingle();

#if APPLOVIN || LEVELPLAY || ADMOB
            Container.BindInterfacesAndSelfTo<LevelCompleteHandler>().AsSingle();
#endif
            
            Container.Bind<ILevelFactory>().To<LevelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelSpawnService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerSplineMover>().AsSingle();
            Container.BindInterfacesTo<PlayerSpawnService>().AsSingle();

            Container.Bind<EnemyPlacer>().AsSingle();
        }
    }
}