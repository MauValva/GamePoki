using Global.Scripts.Generated;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/PoolModule/" + nameof(PoolModuleInstaller),
    fileName = nameof(PoolModuleInstaller), order = 0)]
    public class PoolModuleInstaller : ScriptableObjectInstaller<PoolModuleInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PoolObjectsContainer>().AsSingle();
            Container.BindInterfacesTo<InteractorPoolContainer>().AsSingle();
        }
    }
}
