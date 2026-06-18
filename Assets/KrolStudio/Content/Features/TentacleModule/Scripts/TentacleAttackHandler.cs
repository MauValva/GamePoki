using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class TentacleAttackHandler
    {
        private readonly TentacleConfig _config;
        private readonly EnemyTransformModel _enemyTransformModel;
        private readonly ITentacleAnimator _animator;
        private readonly EnemyStatsCalculator _enemyStats;
        private readonly EnemyDamageable _enemyDamageable;

        private float timer;
        private Collider[] results = new Collider[1];
        RaycastHit[] hits = new RaycastHit[1];
        private bool isAttack;
        private int targetCount;

        [Inject]
        public TentacleAttackHandler(
            TentacleConfig config,
            PlayerTransformModel playerTransformModel,
            EnemyTransformModel enemyTransformModel,
            ITentacleAnimator animator,
            EnemyStatsCalculator enemyStats,
            EnemyDamageable enemyDamageable)
        {
            _config = config;
            _enemyTransformModel = enemyTransformModel;
            _animator = animator;
            _enemyStats = enemyStats;
            _enemyDamageable = enemyDamageable;
        }

        public void Tick()
        {
            timer += Time.deltaTime;
            if (timer < _config.attackInterval)
                return;

            timer = 0f;
            if (!isAttack)
                FindTarget();
        }

        public void FindTarget()
        {
            int count = Physics.OverlapSphereNonAlloc(
               _enemyTransformModel.Position,
               _config.triggerDistance,
               results,
               _config.detectableTargets,
               QueryTriggerInteraction.Collide
            );

            if (count > 0)
            {
                Vector3 origin = _enemyTransformModel.Position + _enemyTransformModel.Transform.up;

                targetCount = Physics.SphereCastNonAlloc(
                       origin,
                       _config.sphereCastRadius,
                       _enemyTransformModel.Transform.forward,
                       hits,
                       _config.sphereCastDistance,
                       //config.damageableTargets,
                       _config.detectableTargets,
                       QueryTriggerInteraction.Collide
                   );

                if (targetCount > 0)
                {
                    isAttack = true;
                    _animator.PlayAttack(HandleAttack);
                }
            }
        }

        public void HandleAttack()
        {
            for (int i = 0; i < targetCount; i++)
            {
                if (hits[i].collider.gameObject.TryGetComponent<PlayerDamageable>(out var damageable))
                    Attack(damageable);

                hits[i] = default;
            }

            isAttack = false;
            targetCount = 0;
            timer = 0f;
        }

        private void Attack(IDamageable damageable)
        {
            damageable.Damage(_enemyStats.GetCurrentDamage(_enemyDamageable.Ratio));
        }
    }
}