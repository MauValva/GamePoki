using Zenject;

namespace KrolStudio
{
    public class PlayerStatesFactory : StatesFactory
    {
        [Inject]
        public PlayerStatesFactory(IInstantiator instantiator)
            : base(instantiator)
        {
        }
    }
}