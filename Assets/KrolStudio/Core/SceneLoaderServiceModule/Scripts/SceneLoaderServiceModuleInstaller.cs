using Zenject;

namespace KrolStudio
{
    public class SceneLoaderServiceModuleInstaller : Installer<SceneLoaderServiceModuleInstaller> 
    {
        public override void InstallBindings() 
        {
            Container.Bind<ISceneLoaderService>()
                .FromInstance(new SceneLoaderServiceFacade(new BuildInSceneLoaderService(), new AddressablesSceneLoaderService()))
                .AsSingle();
        }
    }
}