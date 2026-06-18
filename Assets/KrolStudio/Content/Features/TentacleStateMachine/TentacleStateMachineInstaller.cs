using Zenject;

namespace KrolStudio
{
    public class TentacleStateMachineInstaller : Installer<TentacleStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<TentacleStateMachine>().AsSingle();

            Container.Bind<StateMachine>()
                .To<TentacleStateMachine>()
                .FromResolve();

            Container.Bind<ITentacleStateService>()
               .To<TentacleStateService>()
               .AsSingle();
        }
    }
}