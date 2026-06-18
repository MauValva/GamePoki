using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class PlayerUpgradeState : IState
    {
        private readonly IMovementService _movement;
        private readonly CameraSwitcherProxy _cameraSwitcher;

        [Inject]
        public PlayerUpgradeState(
            IMovementService movement,
            CameraSwitcherProxy cameraSwitcher)
        {
            _movement = movement;
            _cameraSwitcher = cameraSwitcher;
        }

        public UniTask Enter()
        {
            _movement.Restart();
            _cameraSwitcher.SwitchToStart();
            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}