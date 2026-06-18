using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class EnemyChaseTransitionChecker
    {
        private readonly PlayerTransformModel _playerTransform;
        private readonly EnemyTransformModel _enemyTransform;
        private readonly PlayerChaseTargetModel _chaseTarget;
        private readonly EnemyConfig _config;

        [Inject]
        public EnemyChaseTransitionChecker(PlayerTransformModel playerTransform,
                                           EnemyTransformModel enemyTransform,
                                           PlayerChaseTargetModel chaseTarget,
                                           EnemyConfig config)
        {
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _chaseTarget = chaseTarget;
            _config = config;
        }

        public Vector3 PlayerPosition => _playerTransform.Position;

        public bool CanTransitionToChase() =>
            DistanceToPlayerSq() <= _config.triggerDistance * _config.triggerDistance;

        public bool IsPossibleToChase() =>
            _chaseTarget.Value.IsPossibleToChase();

        public bool IsTargetLost() =>
            IsBehindTarget() && DistanceToPlayerSq() > _config.chaseStopDistance * _config.chaseStopDistance;

        public bool IsBehindTarget()
        {
            Vector3 toTarget = (PlayerPosition - _enemyTransform.Position).normalized;
            return Vector3.Dot(_playerTransform.Transform.forward, toTarget) > _config.backAngleThreshold; // backAngleThreshold Provides a small "tolerance angle"
        }

        public float DistanceToPlayerSq() => 
            _enemyTransform.DistanceToSq(_playerTransform.Position);
    }
}