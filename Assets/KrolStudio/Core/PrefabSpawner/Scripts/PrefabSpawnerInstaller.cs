using Zenject;

namespace KrolStudio
{
    public class PrefabSpawnerInstaller : Installer<PrefabSpawnerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PrefabsFactory>().AsSingle();
        }
    }
}