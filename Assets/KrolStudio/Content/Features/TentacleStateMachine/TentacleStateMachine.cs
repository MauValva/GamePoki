
namespace KrolStudio
{
    public class TentacleStateMachine : StateMachine
    {
        public override void RegisterStates(IStatesFactory statesFactory)
        {
            RegisterState(() => statesFactory.Create<TentacleIdleState>());
            RegisterState(() => statesFactory.Create<TentacleAttackState>());
            RegisterState(() => statesFactory.Create<TentacleDeadState>());
        }
    }
}