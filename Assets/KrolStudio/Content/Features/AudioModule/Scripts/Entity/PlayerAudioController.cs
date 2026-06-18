using System;
using Zenject;

namespace KrolStudio
{
    public class PlayerAudioController : IInitializable, IDisposable
    {
        private readonly IAudioService _audioService;
        private readonly PlayerDamageable _damageable;
        private readonly IMovementService _movement;
        private readonly TurretShooting _turretShooting;
        private readonly UpgradeLevelController _upgradeLevel;
        private readonly PlayerTransformModel _transformModel;

        private SoundEmitter _runningCarEmitter;

        public PlayerAudioController(
            IAudioService audioService,
            IMovementService movement,
            PlayerDamageable damageable,
            TurretShooting turretShooting,
            UpgradeLevelController upgradeLevel,
            PlayerTransformModel transformModel)
        {
            _audioService = audioService;
            _damageable = damageable;
            _movement = movement;
            _turretShooting = turretShooting;
            _upgradeLevel = upgradeLevel;
            _transformModel = transformModel;
        }

        public void Initialize()
        {
            _damageable.OnDamaged += OnDamaged;
            _movement.SplineMoveStarted += OnMoveStarted;
            _movement.SplineMoveCompleted += OnMoveCompleted;
            _damageable.OnKilled += OnKilled;
            _damageable.OnKilled += OnMoveCompleted;
            _turretShooting.OnShot += OnShot;
        }

        public void Dispose()
        {
            _damageable.OnDamaged -= OnDamaged;
            _movement.SplineMoveStarted -= OnMoveStarted;
            _movement.SplineMoveCompleted -= OnMoveCompleted;
            _damageable.OnKilled -= OnKilled;
            _turretShooting.OnShot -= OnShot;
        }

        private void OnShot() =>
            _audioService.Play(GameConstants.Sounds.TurretSoundByLevel[_upgradeLevel.GetLevel(PartType.Turret)], _transformModel.Position);

        private void OnKilled() =>
            _audioService.Play(GameConstants.Sounds.CarCrash, _transformModel.Position);

        private void OnMoveStarted() =>
            _runningCarEmitter = _audioService.Play(GameConstants.Sounds.RunningCar, _transformModel.Transform);

        private void OnDamaged(float damage) =>
            _audioService.Play(GameConstants.Sounds.CarDamage, _transformModel.Position);

        private void OnMoveCompleted() =>
            _audioService.Stop(_runningCarEmitter);
    }
}