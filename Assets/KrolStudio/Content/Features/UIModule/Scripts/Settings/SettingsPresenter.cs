using System;
using Zenject;

namespace KrolStudio
{
    public class SettingsPresenter : IInitializable, IDisposable
    {
        private readonly SettingsView _view;
        private readonly IUIScreenNavigationService _navigation;
        private readonly IGameFlowService _gameFlowService;
        private readonly IAudioSettingsService _audioSettings;
        private readonly IPauseManager _pauseManager;
        private readonly IVibrationService _vibrateService;
        private readonly SignalBus _signalBus;

        [Inject]
        public SettingsPresenter(
            UIManager manager,
            IGameFlowService gameFlowService,
            IUIScreenNavigationService navigation,
            IAudioSettingsService audioSettings,
            IPauseManager pauseManager,
            IVibrationService vibrateService,
            SignalBus signalBus)
        {
            _view = manager.GetScreen<SettingsView>();
            _gameFlowService = gameFlowService;
            _navigation = navigation;
            _audioSettings = audioSettings;
            _pauseManager = pauseManager;
            _signalBus = signalBus;
            _vibrateService = vibrateService;
        }

        public void Initialize()
        {
            _view.ResumeAction += Resume;
            _view.RestartAction += Restart;

            _signalBus.Subscribe<SettingsLoadedSignal>(OnSettingsLoaded);
            _signalBus.Fire(new RequestSettingsSignal());
        }

        public void Dispose()
        {
            _view.ResumeAction -= Resume;
            _view.RestartAction -= Restart;

            _signalBus.Unsubscribe<SettingsLoadedSignal>(OnSettingsLoaded);
        }

        private void Resume()
        {
            ApplySettings();

            _pauseManager.Resume();
            FireChanged();
            _navigation.PopPopup();
        }

        private void Restart()
        {
            ApplySettings();

            _pauseManager.Resume();
            _signalBus.Fire<GameplayFinishedSignal>();
            FireChanged();
            _navigation.PopPopup();
            _gameFlowService.EnterGameplay<UpgradeScreenView>();
        }

        private void OnSettingsLoaded(SettingsLoadedSignal signal)
        {
            _view.SetMusicView(signal.MusicVolume);
            _view.SetSfxView(signal.SfxVolume);
            _view.SetVibrateView(signal.Vibrate);

            SetMusicVolume(signal.MusicVolume);
            SetSfxVolume(signal.SfxVolume);
            SetVibrate(signal.Vibrate);
        }

        private void ApplySettings()
        {
            _audioSettings.SetMusicVolume(_view.GetMusicView());
            _audioSettings.SetSfxVolume(_view.GetSfxView());
            _vibrateService.SetVibrate(_view.GetVibrateView());
        }

        public void SetMusicVolume(float value) =>
            _audioSettings.SetMusicVolume(_view.GetMusicView());
        
        public void SetSfxVolume(float value) =>
            _audioSettings.SetSfxVolume(_view.GetSfxView());
        
        public void SetVibrate(bool value) =>
            _vibrateService.SetVibrate(_view.GetVibrateView());
    
        private void FireChanged() =>
            _signalBus.Fire(new SettingsChangedSignal
            {
                MusicVolume = _audioSettings.GetMusicVolume(),
                SfxVolume = _audioSettings.GetSfxVolume(),
                Vibrate = _vibrateService.GetVibrate(),
            });
    }
}