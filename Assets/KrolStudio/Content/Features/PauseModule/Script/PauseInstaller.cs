using Zenject;

namespace KrolStudio
{
    public class PauseInstaller : Installer<PauseInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPauseManager>().To<PauseManager>().AsSingle();
        }
    }
}