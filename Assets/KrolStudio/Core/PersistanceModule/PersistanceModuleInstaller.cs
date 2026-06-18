using Zenject;

namespace KrolStudio
{
    public class PersistanceModuleInstaller : Installer<PersistanceModuleInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WalletPersistor>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerPersistor>().AsSingle();
            Container.BindInterfacesAndSelfTo<SettingsPersistor>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProgressPersistor>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoardPersistor>().AsSingle();
        }
    }
}
