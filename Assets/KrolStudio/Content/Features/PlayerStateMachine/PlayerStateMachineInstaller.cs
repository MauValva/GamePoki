using Zenject;

namespace KrolStudio
{
    public class PlayerStateMachineInstaller : Installer<PlayerStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerStateMachine>().AsSingle();

            Container.Bind<StateMachine>()
                .To<PlayerStateMachine>()
                .FromResolve();

            Container.Bind<IPlayerStateService>()
               .To<PlayerStateService>()
               .AsSingle();
        }
    }
}