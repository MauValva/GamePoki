using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PauseManager : IPauseManager
    {
        private readonly SignalBus _signalBus;

        public bool IsPaused { get; private set; }

        public PauseManager(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Pause()
        {
            if (IsPaused) return;

            Time.timeScale = 0f;
            AudioListener.pause = true;
            IsPaused = true;

            _signalBus.Fire<GamePausedSignal>();
        }

        public void Resume()
        {
            if (!IsPaused) return;

            Time.timeScale = 1f;
            AudioListener.pause = false;
            IsPaused = false;

            _signalBus.Fire<GameResumedSignal>();
        }

        public void TogglePause()
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }
}