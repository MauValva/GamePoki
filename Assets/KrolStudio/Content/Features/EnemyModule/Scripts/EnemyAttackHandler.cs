using UnityEngine;

namespace KrolStudio
{
    public class EnemyAttackHandler
    {
        private readonly PlayerDamageableModel _playerDamageable;
        private readonly EnemyDamageable _enemyDamageable;
        private readonly EnemyConfig _config;
        private readonly EnemyTransformModel _enemyTransform;
        private readonly PlayerChaseTargetModel _chaseTarget;
        private readonly EnemyStatsCalculator _enemyStats;


        public EnemyAttackHandler(
            PlayerDamageableModel playerDamageable,
            EnemyDamageable enemyDamageable,
            EnemyTransformModel enemyTransform,
            PlayerChaseTargetModel chaseTarget,
            EnemyConfig config,
            EnemyStatsCalculator enemyStats)
        {
            _playerDamageable = playerDamageable;
            _enemyDamageable = enemyDamageable;
            _enemyTransform = enemyTransform;
            _chaseTarget = chaseTarget;
            _config = config;
            _enemyStats = enemyStats;
        }

        public bool TryAttack()
        {
            float distSq = RemainingDistanceToPoint();
            if (distSq >= _config.attackDistance * _config.attackDistance)
                return false;

            DealDamageAndDie();
            return true;
        }

        public bool TryBehindAttack(bool isBehindTarget, float distanceSq) =>
            isBehindTarget && distanceSq < _config.behindAttackDistance * _config.behindAttackDistance;

        public void DealDamageAndDie()
        {
            _playerDamageable.Value.Damage(_enemyStats.GetCurrentDamage(_enemyDamageable.Ratio));
            _enemyDamageable.Damage(_enemyDamageable.MaxHealth);
        }

        private float RemainingDistanceToPoint()
        {
            Vector3 target = _chaseTarget.Value.GetClosestTargetPoint(_enemyTransform.Position);
            return (_enemyTransform.Position - target).sqrMagnitude;
        }
    }
}