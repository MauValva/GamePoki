using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class TentacleIdleState : IState, IUpdatableState
    {
        private readonly TentacleTriggerChecker _triggerChecker; 
        private readonly ITentacleStateService _stateService;
        private readonly PlayerTransformModel _playerTransformModel;
        private readonly EnemyTransformModel _enemyTransformModel;
        private readonly TentacleRotator _rotator;

        [Inject]
        public TentacleIdleState(TentacleTriggerChecker triggerChecker,
                                 ITentacleStateService stateService,
                                 PlayerTransformModel playerTransformModel,
                                 EnemyTransformModel enemyTransformModel,
                                 TentacleRotator rotator)
        {
            _triggerChecker = triggerChecker;
            _stateService = stateService;
            _playerTransformModel = playerTransformModel;
            _enemyTransformModel = enemyTransformModel;
            _rotator = rotator;
        }

        public UniTask Enter() => default;
        public UniTask Exit() => default;

        public void Update()
        {
            _rotator.LookAtPlayerY();

            if (_triggerChecker.CanTriggered())
                _stateService.EnterAttack().Forget();
        }
    }
}
