using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class GameplayPoolInstaller : Installer<GameplayPoolInstaller>
    {
        public override void InstallBindings()
        {
            var assetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            Container.Bind<GameplayPoolsConfig>()
               .FromScriptableObject(assetLoaderService.LoadAsset<GameplayPoolsConfig>(Address.Configurations.GameplayPoolsConfig))
               .AsSingle();

            Container.BindInterfacesTo<GameplayPoolsInitializer>().AsSingle();
        }
    }
}

