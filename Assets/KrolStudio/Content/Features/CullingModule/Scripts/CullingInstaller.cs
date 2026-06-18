using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class CullingInstaller : Installer<CullingInstaller>
    {
        public override void InstallBindings()
        {
            // Culling
            var assetLoader = Container.Resolve<IAddressablesAssetLoaderService>();
            Container.Bind<CullingConfig>()
                .FromScriptableObject(assetLoader.LoadAsset<CullingConfig>(Address.Configurations.CullingConfig))
                .AsSingle();

            Container.Bind<CullingRegistry>().AsSingle();
            Container.BindInterfacesTo<CullingService>().AsSingle();
        }
    }
}