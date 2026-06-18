using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KrolStudio
{
    public class EnemyIdleState : IState, IUpdatableState
    {
        private readonly EnemyChaseTransitionChecker _transitionChecker;
        private readonly IEnemyStateService _enemyStateService;
        private readonly EnemyConfig _config;
        private readonly IEnemyAnimator _enemyAnimator;

        float t;
        float duration;

        public EnemyIdleState(IEnemyStateService enemyStateService, 
                              EnemyConfig config,
                              EnemyChaseTransitionChecker transitionChecker,
                              IEnemyAnimator enemyAnimator)
        {
            _enemyStateService = enemyStateService;
            _transitionChecker = transitionChecker;
            _config = config;
            _enemyAnimator = enemyAnimator;
        }


        public UniTask Enter()
        {
            _enemyAnimator.PlayMovement(false);
            t = 0;
            duration = Random.Range(_config.idleDuration.x, _config.idleDuration.y);
            return default;
        }

        public UniTask Exit() => default;

        bool CanTransitionToWander()
        {
            t += Time.deltaTime;
            return t >= duration;
        }

        public void Update()
        {
            if (!_transitionChecker.IsPossibleToChase())
            {
                _enemyStateService.EnterInactive();
                return;
            }

            if(CanTransitionToWander())
            {
                _enemyStateService.EnterWander();
                return;
            }

            if(_transitionChecker.CanTransitionToChase())
            {
                _enemyStateService.EnterChase();
                return;
            }
        }
    }
}