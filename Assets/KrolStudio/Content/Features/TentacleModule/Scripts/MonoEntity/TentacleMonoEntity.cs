using Zenject;
using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public class TentacleMonoEntity : MonoEntity
    {
        private TentacleStatesFactory _statesFactory;
        private ITentacleStateService _stateService;
        private ICullable _cullable;

        [Inject]
        private void Construct(
            TentacleStatesFactory statesFactory,
            ITentacleStateService stateService)
        {
            _statesFactory = statesFactory;
            _stateService = stateService;
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
            _stateService.EnterIdle().Forget();
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