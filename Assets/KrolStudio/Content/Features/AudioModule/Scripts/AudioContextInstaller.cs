using Zenject;

namespace KrolStudio
{
    public class AudioContextInstaller : Installer<PersistanceModuleInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MusicController>().AsSingle();
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
        }
    }
}