using Zenject;

namespace KrolStudio
{
    public class VibrationInstaller : Installer<VibrationInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IVibrationService>().To<VibrationService>().AsSingle();
        }
    }
}