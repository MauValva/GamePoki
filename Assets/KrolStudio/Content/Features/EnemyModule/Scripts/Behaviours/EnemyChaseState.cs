using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public class EnemyChaseState : IState, IUpdatableState
    {
        private readonly IEnemyStateService _enemyStateService;
        private readonly EnemyConfig _config;
        private readonly IEnemyAnimator _animator;
        private readonly EnemyChaseTransitionChecker _transitionChecker;
        private readonly EnemyMover _mover;
        private readonly EnemyAttackHandler _attackHandler;
        private readonly IAudioService _audioService;
        private readonly EnemyTransformModel _enemyTransform;

        public EnemyChaseState(IEnemyStateService enemyStateService, 
                               EnemyConfig config,
                               IEnemyAnimator animator,
                               EnemyChaseTransitionChecker transitionChecker,
                               EnemyMover mover,
                               EnemyAttackHandler attackHandler,
                               IAudioService audioService,
                               EnemyTransformModel enemyTransform)
        {
            _enemyStateService = enemyStateService;
            _config = config;
            _animator = animator;
            _transitionChecker = transitionChecker;
            _mover = mover;
            _attackHandler = attackHandler;
            _audioService = audioService;
            _enemyTransform = enemyTransform;
        }

        public UniTask Enter()
        {
            _audioService.Play(GameConstants.Sounds.GetRandomStickmanReaction(), _enemyTransform.Position);
            _animator.PlayMovement(true);
            return default;
        }

        public UniTask Exit()
        {
            _animator.PlayMovement(false);
            return default;
        }

        public void Update()
        {
            _animator.ChangingMovement(_config.runningThreshold, _config.accelerationMultiplier);
            _mover.MoveTowards(_transitionChecker.PlayerPosition, _config.speed, _config.rotationSpeed);

            if (_attackHandler.TryAttack()) return;
            
            if (_attackHandler.TryBehindAttack(_transitionChecker.IsBehindTarget(), _transitionChecker.DistanceToPlayerSq()))
                _enemyStateService.EnterAttack();

            if (!_transitionChecker.IsPossibleToChase() || _transitionChecker.IsTargetLost())
                _enemyStateService.EnterInactive();

        }
    }
}