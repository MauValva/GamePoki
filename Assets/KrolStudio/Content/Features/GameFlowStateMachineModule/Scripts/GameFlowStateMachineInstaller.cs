using Zenject;

namespace KrolStudio
{
    public class GameFlowStateMachineInstaller : Installer<GameFlowStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameFlowStateMachine>().AsSingle();

            Container.Bind<IGameFlowService>()
                .To<GameFlowService>()
                .AsSingle();
        }
    }
}