using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class TentacleAttackState : IState, IUpdatableState
    {
        private readonly TentacleAttackHandler _attack;
        private readonly TentacleRotator _rotator;
        private readonly TentacleTriggerChecker _triggerChecker;
        private readonly ITentacleStateService _stateService;

        [Inject]
        public TentacleAttackState(
            TentacleAttackHandler attack,
            TentacleRotator rotator,
            TentacleTriggerChecker triggerChecker,
            ITentacleStateService stateService)
        {
            _attack = attack;
            _rotator = rotator;
            _triggerChecker = triggerChecker;
            _stateService = stateService;
        }

        public UniTask Enter() => default;
        public UniTask Exit() => default;

        public void Update()
        {
            //GizmosUtility.DrawCircle(_enemyTransformModel.Position, _config.triggerDistance, _enemyTransformModel.Transform.up, Color.green);
            _rotator.LookAtPlayerY();

            if (!_triggerChecker.CanTriggered())
            {
                _stateService.EnterIdle().Forget();
                return;
            }

            _attack.Tick();
        }
    }
}