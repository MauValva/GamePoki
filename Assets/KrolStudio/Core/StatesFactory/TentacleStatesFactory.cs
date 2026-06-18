using Zenject;

namespace KrolStudio
{
    public class TentacleStatesFactory : StatesFactory
    {
        [Inject]
        public TentacleStatesFactory(IInstantiator instantiator)
            : base(instantiator)
        {
        }
    }
}