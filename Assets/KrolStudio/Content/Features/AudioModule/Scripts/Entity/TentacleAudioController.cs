using System;
using Zenject;

namespace KrolStudio
{
    public class TentacleAudioController : IInitializable, IDisposable
    {
        private readonly IAudioService _audioService;
        private readonly EnemyDamageable _damageable;

        public TentacleAudioController(
            IAudioService audioService,
            EnemyDamageable damageable)
        {
            _audioService = audioService;
            _damageable = damageable;
        }

        public void Initialize()
        {
            _damageable.OnKilled += OnKilled;
        }

        public void Dispose()
        {
            _damageable.OnKilled -= OnKilled;
        }
     
        private void OnKilled() =>
            _audioService.Play(GameConstants.Sounds.TentacleDeath, _damageable.transform.position);
    }
}