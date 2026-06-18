using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public class EnemyAttackState : IState, IUpdatableState
    {
        private readonly IEnemyAnimator _animator;
        private readonly EnemyMover _mover;
        private readonly EnemyAttackHandler _attackHandler;
        private readonly EnemyTransformModel _enemyTransform;
        private readonly PlayerTransformModel _playerTransform;
        private readonly EnemyConfig _config;
        private readonly IAudioService _audioService;

        public EnemyAttackState(
            IEnemyAnimator animator,
            EnemyMover mover,
            EnemyAttackHandler attackHandler,
            PlayerTransformModel playerTransform,
            EnemyTransformModel enemyTransform,
            EnemyConfig config,
            IAudioService audioService)
        {
            _animator = animator;
            _mover = mover;
            _attackHandler = attackHandler;
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _config = config;
            _audioService = audioService;
        }

        public UniTask Enter()
        {
            _audioService.Play(GameConstants.Sounds.StickmanAttack, _enemyTransform.Position);
            _animator.PlayBehindAttack(_attackHandler.DealDamageAndDie);
            return default;
        }

        public UniTask Exit() => default;

        public void Update()
        {
            _mover.MoveTowards(_playerTransform.Position, _config.speed, _config.rotationSpeed);
        }
    }
}