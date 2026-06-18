using System;
using Zenject;

namespace KrolStudio
{
    // MusicController - Only for background music.
    public class MusicController : IInitializable, IDisposable
    {
        private readonly IAudioService _audioService;
        private readonly SignalBus _signalBus;

        private SoundEmitter _bgEmitter;

        public MusicController(
            IAudioService audioService, 
            SignalBus signalBus)
        {
            _audioService = audioService;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<GameplayStartedSignal>(OnGameplayStarted);
            _signalBus.Subscribe<GameplayFinishedSignal>(OnGameplayFinished);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GameplayStartedSignal>(OnGameplayStarted);
            _signalBus.Unsubscribe<GameplayFinishedSignal>(OnGameplayFinished);
        }

        private void OnGameplayStarted()
        {
            _bgEmitter = _audioService.Play(GameConstants.Sounds.Main);
            //_audioService.PlaySoundFading(GameConstants.Sounds.Main, 0f, 1f, 1.5f);
        }

        private void OnGameplayFinished()
        {
            _audioService.Stop(_bgEmitter);
            //_audioManager.StopSoundFading(GameConstants.Sounds.Main, 1f, 0f, 1f);
        }
    }
}