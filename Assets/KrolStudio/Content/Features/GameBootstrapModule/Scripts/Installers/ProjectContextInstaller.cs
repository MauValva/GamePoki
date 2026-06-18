using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/GameBootstrap/" + nameof(ProjectContextInstaller),
    fileName = nameof(ProjectContextInstaller), order = 0)]
    public class ProjectContextInstaller : ScriptableObjectInstaller<ProjectContextInstaller>
    {
        public override void InstallBindings()
        {
            BindGlobalSignals();

            SceneLoaderServiceModuleInstaller.Install(Container);
            AssetLoaderInstaller.Install(Container);
            GameFlowStateMachineInstaller.Install(Container);

            Container.Bind<ILogService>().To<LogService>().AsSingle();
            Container.Bind<IStatesFactory>().To<StatesFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<LoadingCurtain>()
                .FromComponentInHierarchy()
                .AsSingle();
        }


        private void BindGlobalSignals()
        {
            SignalBusInstaller.Install(Container);      
            GlobalSignalsInstaller.Install(Container);
        }
    }
}