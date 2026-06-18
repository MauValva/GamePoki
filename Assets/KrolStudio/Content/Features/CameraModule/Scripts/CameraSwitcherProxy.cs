
namespace KrolStudio
{
    public class CameraSwitcherProxy
    {
        private ICameraSwitcher _real;

        public void SetReal(ICameraSwitcher real) =>
            _real = real;

        public void SwitchToFinish() =>
            _real.SwitchToFinish();

        public void SwitchToFollow() =>
            _real.SwitchToFollow();

        public void SwitchToStart() =>
            _real.SwitchToStart();
    }
}