using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class PlayerStateService : IPlayerStateService
    {
        private readonly PlayerStateMachine _playerStateMachine;

        [Inject]
        public PlayerStateService(PlayerStateMachine stateMachine)
        {
            _playerStateMachine = stateMachine;
        }

        public async UniTask EnterUpgrade() =>
           await _playerStateMachine.Enter<PlayerUpgradeState>();

        public async UniTask EnterIdle() =>
            await _playerStateMachine.Enter<PlayerIdleState>();

        public async UniTask EnterRun() =>
            await _playerStateMachine.Enter<PlayerRunState>();

        public async UniTask EnterDead() =>
            await _playerStateMachine.Enter<PlayerDeadState>();

        public async UniTask EnterFinish() =>
            await _playerStateMachine.Enter<PlayerFinishState>();
    }
}