using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KrolStudio
{
    public class EnemyWanderState : IState, IUpdatableState
    {
        private readonly IEnemyStateService _enemyStateService;
        private readonly EnemyConfig _config;
        private readonly EnemyChaseTransitionChecker _transitionChecker;
        private readonly IEnemyAnimator _enemyAnimator;
        private readonly EnemySpawnPointModel _spawnPoint;
        private readonly EnemyMover _enemyMover;

        Vector3 _destinationPoint;

        public EnemyWanderState(IEnemyStateService enemyStateService,
                                EnemyConfig config,
                                EnemyChaseTransitionChecker transitionChecker,
                                IEnemyAnimator enemyAnimator,
                                EnemySpawnPointModel spawnPoint,
                                EnemyMover enemyMover)
        {
            _enemyStateService = enemyStateService;
            _transitionChecker = transitionChecker;
            _config = config;
            _enemyAnimator = enemyAnimator;
            _spawnPoint = spawnPoint;
            _enemyMover = enemyMover;
        }

        public UniTask Enter()
        {
            _enemyAnimator.PlayMovement(true);
            _destinationPoint = GetRandomPointInCircle(_spawnPoint.SpawnPosition, _config.wanderZoneRadius);

            return default;
        }

        public UniTask Exit()
        {
            _enemyAnimator.PlayMovement(false);
            return default;
        }

        public void Update()
        {
            if(!_transitionChecker.IsPossibleToChase())
            {
                _enemyStateService.EnterInactive().Forget();
                return;
            }

            _enemyAnimator.ChangingMovement(_config.walkingThreshold, _config.accelerationMultiplier);

            if (_transitionChecker.CanTransitionToChase())
            {
                _enemyStateService.EnterChase().Forget();
                return;
            }

            _enemyMover.MoveTowards(_destinationPoint, _config.walkingSpeed, _config.rotationSpeed);

            if (CanTransitionToIdle())
            {
                _enemyStateService.EnterIdle().Forget();
                return;
            }
        }

        bool CanTransitionToIdle() =>
            _enemyMover.HasReachedPoint(_destinationPoint, _config.wanderArrivalDistance);

        Vector3 GetRandomPointInCircle(Vector3 center, float radius)
        {
            Vector2 randomPoint2D = Random.insideUnitCircle * radius;
            return center + new Vector3(randomPoint2D.x, 0f, randomPoint2D.y);
        }
    }
}