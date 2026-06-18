using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class PlayerIdleState : IState
    {
        private readonly CameraSwitcherProxy _cameraSwitcher;
        private readonly IMovementService _movement;
        private readonly DisplayablesModel _displayables;

        [Inject]
        public PlayerIdleState(
            CameraSwitcherProxy cameraSwitcher, 
            IMovementService movement,
            DisplayablesModel displayables)
        {
            _cameraSwitcher = cameraSwitcher;
            _movement = movement;
            _displayables = displayables;
        }

        public UniTask Enter()
        {
            foreach (var d in _displayables.All)
                d.Display(false);

            _movement.Restart();
            _cameraSwitcher.SwitchToStart();
            return default;
        }

        public UniTask Exit()
        {
            foreach (var d in _displayables.All)
                d.Display(true);

            return default;
        }
    }
}