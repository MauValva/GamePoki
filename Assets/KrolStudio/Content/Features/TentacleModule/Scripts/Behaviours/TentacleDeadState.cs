using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class TentacleDeadState : IState
    {
        private readonly ITentacleAnimator _animator;

        [Inject]
        public TentacleDeadState(ITentacleAnimator animator)
        {
            _animator = animator;
        }

        public UniTask  Enter()
        {
            _animator.PlayDeath();
            return default;
        }

        public UniTask Exit() => default;
    }
}