using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class CameraSwitchTriggerAction : MonoBehaviour, ITriggerAction
    {
        private CameraSwitcherProxy _cameraSwitcher;

        [Inject]
        private void Construct(CameraSwitcherProxy cameraSwitcher) =>
            _cameraSwitcher = cameraSwitcher;

        public void Execute() =>
            _cameraSwitcher.SwitchToFinish();
    }
}
