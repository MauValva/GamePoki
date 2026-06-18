using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class EnemyStateService : IEnemyStateService
    {
        private readonly EnemyStateMachine _enemyStateMachine;

        [Inject]
        public EnemyStateService(EnemyStateMachine stateMachine) =>
            _enemyStateMachine = stateMachine;

        public async UniTask EnterAttack() =>
            await _enemyStateMachine.Enter<EnemyAttackState>();

        public async UniTask EnterChase() =>
            await _enemyStateMachine.Enter<EnemyChaseState>();

        public async UniTask EnterDead() =>
            await _enemyStateMachine.Enter<EnemyDeadState>();

        public async UniTask EnterHit() =>
            await _enemyStateMachine.Enter<EnemyHitState>();

        public async UniTask EnterIdle() =>
            await _enemyStateMachine.Enter<EnemyIdleState>();

        public async UniTask EnterInactive() =>
            await _enemyStateMachine.Enter<EnemyInactiveState>();

        public async UniTask EnterWander() =>
            await _enemyStateMachine.Enter<EnemyWanderState>();

        public async UniTask EnterPrevious() =>
            await _enemyStateMachine.EnterPrevious();
    }
}