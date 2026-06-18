using Zenject;

namespace KrolStudio
{
    public class EnemyStateMachineInstaller : Installer<EnemyStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<EnemyStateMachine>().AsSingle();

            Container.Bind<StateMachine>()
                .To<EnemyStateMachine>()
                .FromResolve();

            Container.Bind<IEnemyStateService>()
               .To<EnemyStateService>()
               .AsSingle();
        }
    }
}