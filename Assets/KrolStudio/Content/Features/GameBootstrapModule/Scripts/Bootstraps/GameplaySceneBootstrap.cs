using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class GameplaySceneBootstrap : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        void Start()
        {
            _signalBus.Fire<GameplayStartedSignal>();
        }
    }
}