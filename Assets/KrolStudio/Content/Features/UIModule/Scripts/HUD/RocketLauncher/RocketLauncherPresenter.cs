using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    // Presenter — reload logic, orchestration
    public class RocketLauncherPresenter : IInitializable, IDisposable
    {
        private readonly RocketLauncherView _view;
        private readonly RocketConfig _config;
        private readonly CtsHelper _ctsHelper = new();
        private readonly DisplayablesModel _model;
        private readonly RocketLaunchModel _rocketService;

        private bool _isReloading;
        public bool IsReloading => _isReloading;

        [Inject]
        public RocketLauncherPresenter(
            RocketLauncherView view,
            RocketConfig config,
            DisplayablesModel model,
            RocketLaunchModel rocketService)
        {
            _view = view;
            _config = config;
            _model = model;
            _rocketService = rocketService;
        }

        public void Initialize()
        {
            _view.OnLaunchClicked += OnLaunchClicked;
            _model.Register(_view);
        }

        public void Dispose()
        {
            _view.OnLaunchClicked -= OnLaunchClicked;
            _model.Unregister(_view);
            _ctsHelper.Dispose();
        }

        public void ShowFailureMessage() =>
            _view.ShowFailureMessage();

        private void OnLaunchClicked()
        {
            if (_isReloading) return;

            if (!_rocketService.Value.TryLaunch())
                return;

            ReloadAsync().Forget();
        }

        private async UniTask ReloadAsync()
        {
            try
            {
                _isReloading = true;
                _view.SetLaunchButtonInteractable(false);
                _view.SetReloadActive(true);

                int currentTimer = _config.baseReload;
                _view.SetReloadText(currentTimer);

                while (currentTimer > 0)
                {
                    await UniTask.Delay(
                        TimeSpan.FromSeconds(1f),
                        cancellationToken: _ctsHelper.Token);

                    currentTimer--;
                    _view.SetReloadText(currentTimer);
                }

                _view.SetLaunchButtonInteractable(true);
                _view.SetReloadActive(false);
                _isReloading = false;
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) when (ex is MissingReferenceException || ex is NullReferenceException)
            {
                Debug.LogWarning($"Object destroyed during reload: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error during reload: {ex}");
            }
        }
    }
}