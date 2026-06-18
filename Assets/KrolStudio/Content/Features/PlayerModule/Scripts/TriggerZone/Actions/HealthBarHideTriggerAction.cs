using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class HealthBarHideTriggerAction : MonoBehaviour, ITriggerAction
    {
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus) =>
            _signalBus = signalBus;

        public void Execute() =>
            _signalBus.Fire(new PlayerHUDVisibilitySignal(false));
    }
}