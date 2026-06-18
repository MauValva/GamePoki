using Zenject;
using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public class EnemyMonoEntity : MonoEntity
    {
        private EnemyStatesFactory _statesFactory;
        private IEnemyStateService _enemyStateService;
        private ICullable _cullable;

        [Inject]
        private void Construct(
            EnemyStatesFactory statesFactory,
            IEnemyStateService enemyStateService)
        {
            _statesFactory = statesFactory;
            _enemyStateService = enemyStateService;
        }

        protected override void InitializeContext()
        {
            base.InitializeContext();
            _cullable = GetComponent<ICullable>();
            StateMachine.RegisterStates(_statesFactory);
            SetDefaultState();
        }

        public override void SetDefaultState()
        {
            _enemyStateService.EnterIdle().Forget();
        }

        public void Update()
        {
            if (_cullable != null && _cullable.IsCulled) return;
            StateMachine.UpdateState();
        }

        protected void OnDestroy()
        {
            StateMachine.Dispose();
        }
    }
}