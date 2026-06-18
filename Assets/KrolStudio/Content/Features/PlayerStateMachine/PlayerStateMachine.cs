
namespace KrolStudio
{
    public class PlayerStateMachine : StateMachine
    {
        public override void RegisterStates(IStatesFactory statesFactory)
        {
            //RegisterState(() => statesFactory.Create<PlayerUpgradeState>());
            RegisterState(() => statesFactory.Create<PlayerIdleState>());
            RegisterState(() => statesFactory.Create<PlayerRunState>());
            RegisterState(() => statesFactory.Create<PlayerFinishState>());
            RegisterState(() => statesFactory.Create<PlayerDeadState>());
        }
    }
}