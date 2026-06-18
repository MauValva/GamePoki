using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class PlayerMonoEntity : MonoEntity
    {
        private PlayerStatesFactory _statesFactory;
        private IPlayerStateService _playerStateService;

        [Inject]
        private void Construct(
            PlayerStatesFactory statesFactory,
            IPlayerStateService playerStateService)
        {
            _statesFactory = statesFactory;
            _playerStateService = playerStateService;
        }

        protected override void InitializeContext()
        {
            base.InitializeContext();

            StateMachine.RegisterStates(_statesFactory);

            SetDefaultState();
        }

        public override void SetDefaultState()
        {
            _playerStateService.EnterIdle().Forget();
        }

        public void Update()
        {
            StateMachine.UpdateState();
        }

        protected void OnDestroy()
        {
            StateMachine.Dispose();
        }
    }
}
