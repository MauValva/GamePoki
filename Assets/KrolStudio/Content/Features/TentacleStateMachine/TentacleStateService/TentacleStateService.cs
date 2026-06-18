using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class TentacleStateService : ITentacleStateService
    {
        private readonly TentacleStateMachine _stateMachine;

        [Inject]
        public TentacleStateService(TentacleStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public async UniTask EnterAttack() =>
            await _stateMachine.Enter<TentacleAttackState>();

        public async UniTask EnterDead() =>
            await _stateMachine.Enter<TentacleDeadState>();

        public async UniTask EnterIdle() =>
            await _stateMachine.Enter<TentacleIdleState>();
    }
}