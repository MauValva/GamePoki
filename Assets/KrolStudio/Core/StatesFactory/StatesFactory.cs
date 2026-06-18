using Zenject;

namespace KrolStudio
{
    public class StatesFactory : IStatesFactory
    {
        private IInstantiator instantiator;

        public StatesFactory() { }

        [Inject]
        public StatesFactory(IInstantiator instantiator)
        {
            this.instantiator = instantiator;
        }

        public TState Create<TState>() where TState : IExitableState
        {
            return instantiator.Instantiate<TState>();
        }
    }
}

