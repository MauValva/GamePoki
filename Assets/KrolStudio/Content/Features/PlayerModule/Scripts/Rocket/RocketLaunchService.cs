using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class RocketLaunchService : IRocketLaunchService, IInitializable, IDisposable
    {
        private readonly List<Rocket> _rockets;
        private readonly RocketConfig _config;
        private readonly PlayerTransformModel _playerTransform;
        private readonly PlayerDamageable _playerDamageable;
        private readonly CameraVisibilityChecker _visibilityChecker;
        private readonly RocketLauncherPresenter _launcherPresenter;
        private readonly CtsHelper _ctsHelper = new();
        private readonly RocketLaunchModel _model;
        private readonly IAudioService _audioService;
        private readonly PlayerStatsCalculator _statsCalculator;
        private readonly UpgradeLevelController _upgradeLevel;

        private readonly List<ITargetFocus> _targets = new();
        private readonly List<ITargetFocus> _targetsTemp = new();
        private readonly List<Transform> _transforms = new();
        
        private Collider[] _results;
        private float _timer;

        [Inject]
        public RocketLaunchService(
            List<Rocket> rockets,
            RocketConfig config,
            PlayerTransformModel playerTransform,
            PlayerDamageable playerDamageable,
            CameraVisibilityChecker visibilityChecker,
            RocketLauncherPresenter launcherPresenter,
            RocketLaunchModel model,
            IAudioService audioService,
            PlayerStatsCalculator statsCalculator,
            UpgradeLevelController upgradeLevel)
        {
            _rockets = rockets;
            _config = config;
            _playerTransform = playerTransform;
            _playerDamageable = playerDamageable;
            _visibilityChecker = visibilityChecker;
            _launcherPresenter = launcherPresenter;
            _model = model;
            _audioService = audioService;
            _statsCalculator = statsCalculator;
            _upgradeLevel = upgradeLevel;
        }

        public void Initialize()
        {
            _model.Value = this;
            _results = new Collider[20];

            foreach (var rocket in _rockets)
                rocket.Initialize(_config, _audioService, _statsCalculator, _upgradeLevel);
        }

        public void Dispose() =>
            _ctsHelper.Dispose();

        public void Tick()
        {
            _timer += Time.deltaTime;
            if (_timer < _config.detectionCooldown) return;

            _timer = 0f;

            if (_launcherPresenter.IsReloading)
                DisableFocus();
            else
                FindTarget();
        }

        public void FindTarget()
        {
            int count = Physics.OverlapSphereNonAlloc(
                _playerTransform.Position,
                _config.detectableDistance,
                _results,
                _config.detectableTargets,
                QueryTriggerInteraction.Collide);

            if (count <= 0)
            {
                _targets.Clear();
                return;
            }

            DisableFocus();

            _targetsTemp.Clear();
            _transforms.Clear();

            // We collect only valid transforms up to the count.
            for (int i = 0; i < count; i++)
            {
                if (_results[i] != null && _results[i].transform != null)
                    _transforms.Add(_results[i].transform);
            }

            // Sort by distance — closest first.
            _transforms.Sort((a, b) =>
                (a.position - _playerTransform.Position).sqrMagnitude
                .CompareTo((b.position - _playerTransform.Position).sqrMagnitude));

            foreach (var t in _transforms)
            {
                if (!t.TryGetComponent<ITargetFocus>(out var target)) continue;
                if (!target.IsAlive) continue;
                if (!_visibilityChecker.IsVisible(t.position)) continue;

                target.DisplayFocus(true);
                _targetsTemp.Add(target);

                if (_targetsTemp.Count == _rockets.Count)
                    break;
            }

            _targets.Clear();
            _targets.AddRange(_targetsTemp);
        }

        public void DisableFocus()
        {
            foreach (var target in _targets)
                target.DisplayFocus(false);
        }

        public bool TryLaunch()
        {
            if (!_playerDamageable.IsActive) return false;

            if (_targets.Count == 0)
            {
                _launcherPresenter.ShowFailureMessage();
                return false;
            }

            LaunchAsync().Forget();
            return true;
        }

        private async UniTask LaunchAsync()
        {
            try
            {
                for (int i = 0; i < _rockets.Count; i++)
                {
                    await UniTask.Delay(
                        TimeSpan.FromSeconds(0.1f),
                        cancellationToken: _ctsHelper.Token);

                    int targetIndex = i % _targets.Count;
                    _targets[targetIndex].DisplayFocus(false);
                    _rockets[i].Launch(_targets[targetIndex].Transform);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) when (ex is MissingReferenceException || ex is NullReferenceException)
            {
                Debug.LogWarning($"Object destroyed during launch: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error during launch: {ex}");
            }
            finally
            {
                _targets.Clear();
            }
        }
    }
}