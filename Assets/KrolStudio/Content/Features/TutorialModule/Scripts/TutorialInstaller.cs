using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class TutorialInstaller : Installer<TutorialInstaller>
    {
        public override void InstallBindings()
        {
            var assetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();

            Container.Bind<TutorialConfig>()
               .FromScriptableObject(assetLoaderService.LoadAsset<TutorialConfig>(Address.Configurations.TutorialConfig))
               .AsSingle();

            Container.BindInterfacesAndSelfTo<TutorialService>().AsSingle();
            Container.Bind<TutorialHandView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TutorialStepHandler>().AsSingle();
        }
    }
}