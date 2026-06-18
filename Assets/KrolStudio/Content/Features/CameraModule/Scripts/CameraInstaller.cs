using Zenject;

namespace KrolStudio
{
    public class CameraInstaller : Installer<CameraInstaller> 
    {
        public override void InstallBindings() 
        {
            Container.Bind<CameraSwitcherProxy>().AsSingle();
            Container.Bind<PlayerCameraModel>().AsSingle();
            Container.Bind<CameraVisibilityChecker>().AsSingle();
        }
    }
}