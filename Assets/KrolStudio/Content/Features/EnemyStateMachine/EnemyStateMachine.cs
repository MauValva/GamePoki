
namespace KrolStudio
{
    public class EnemyStateMachine : StateMachine
    {
        public override void RegisterStates(IStatesFactory statesFactory)
        {
            RegisterState(() => statesFactory.Create<EnemyAttackState>());
            RegisterState(() => statesFactory.Create<EnemyChaseState>());
            RegisterState(() => statesFactory.Create<EnemyDeadState>());
            RegisterState(() => statesFactory.Create<EnemyHitState>());
            RegisterState(() => statesFactory.Create<EnemyIdleState>());
            RegisterState(() => statesFactory.Create<EnemyInactiveState>());
            RegisterState(() => statesFactory.Create<EnemyWanderState>());
        }
    }
}