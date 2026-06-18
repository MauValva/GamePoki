using System;
using Zenject;

namespace KrolStudio
{
    public class EnemyAudioController : IInitializable, IDisposable
    {
        private readonly IAudioService _audioService;
        private readonly EnemyDamageable _damageable;

        private SoundEmitter _runningCarEmitter;

        public EnemyAudioController(
            IAudioService audioService,
            EnemyDamageable damageable)
        {
            _audioService = audioService;
            _damageable = damageable;
        }

        public void Initialize()
        {
            _damageable.OnDamaged += OnDamaged;
        }

        public void Dispose()
        {
            _damageable.OnDamaged -= OnDamaged;
        }

        private void OnDamaged(float damage) =>
            _audioService.Play(GameConstants.Sounds.StickmanDamage, _damageable.transform.position);
    }
}