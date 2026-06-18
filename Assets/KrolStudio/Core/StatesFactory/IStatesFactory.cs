
namespace KrolStudio
{
    public interface IStatesFactory
    {
        public TState Create<TState>() where TState : IExitableState;
    }
}