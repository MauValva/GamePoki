using Zenject;

namespace KrolStudio
{
    public class WalletInstaller : Installer<WalletInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<WalletService>().AsSingle();
        }
    }
}