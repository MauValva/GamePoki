using Zenject;

namespace KrolStudio
{
    public class EnemyStatesFactory : StatesFactory
    {
        [Inject]
        public EnemyStatesFactory(IInstantiator instantiator)
            : base(instantiator)
        {
        }
    }
}