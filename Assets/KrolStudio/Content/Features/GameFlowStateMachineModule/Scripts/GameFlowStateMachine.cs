
namespace KrolStudio
{
    public class GameFlowStateMachine : StateMachine
    {
        public override void RegisterStates(IStatesFactory statesFactory)
        {
            RegisterState(() => statesFactory.Create<GlobalGameFlowState>());
            RegisterState(() => statesFactory.Create<GameplayFlowState>());
        }
    }
}
