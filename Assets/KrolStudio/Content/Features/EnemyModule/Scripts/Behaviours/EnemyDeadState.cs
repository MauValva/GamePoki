using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class EnemyDeadState : IState, IUpdatableState
    {
        private readonly IEnemyAnimator _animator;
        private readonly IEnemyEffects _effects;
        private readonly IEnemyRagdoll _ragdoll;
        private readonly PlayerTransformModel _playerTransform;
        private readonly EnemyTransformModel _enemyTransform;
        private readonly EnemyConfig _config;

        private bool _waspushed;

        [Inject]
        public EnemyDeadState(
            IEnemyAnimator animator,
            IEnemyEffects effects,
            IEnemyRagdoll ragdoll,
            PlayerTransformModel playerTransform,
            EnemyTransformModel enemyTransform,
            EnemyConfig config)
        {
            _animator = animator;
            _effects = effects;
            _ragdoll = ragdoll;
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _config = config;
        }

        public UniTask Enter()
        {
            _waspushed = false;
            _effects.PlaySplatRed();
            _animator.Enable(false);
            return default;
        }

        public UniTask Exit()
        {
            _waspushed = false;
            return default;
        }

        public void Update()
        {
            if (_waspushed) return;

            float distSq = _enemyTransform.DistanceToSq(_playerTransform.Position);
            if (distSq > _config.pushRadius * _config.pushRadius) return;

            _ragdoll.Push(_playerTransform.Transform, _config.hitForce);

            _waspushed = true;
        }
    }
}