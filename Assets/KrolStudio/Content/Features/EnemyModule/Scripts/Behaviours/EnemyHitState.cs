using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public class EnemyHitState : IState
    {
        private readonly IEnemyStateService _enemyStateService;
        private readonly IEnemyAnimator _animator;
        private readonly IDamageable _damageable;

        public EnemyHitState(
            IEnemyStateService enemyStateService,
            IEnemyAnimator animator, 
            IDamageable damageable)
        {
            _enemyStateService = enemyStateService;
            _animator = animator;
            _damageable = damageable;
        }

        private void OnHitAnimationEnd()
        {
            if (_damageable.CurrentHealth <= 0) return;
            _enemyStateService.EnterPrevious();
        }

        public UniTask Enter()
        {
            _animator.PlayHit(OnHitAnimationEnd);
            return default;
        }
        
        public UniTask Exit()
        {
            return default;
        }
    }
}