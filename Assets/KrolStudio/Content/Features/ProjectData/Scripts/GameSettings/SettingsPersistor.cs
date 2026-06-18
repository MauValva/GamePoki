using Zenject;

namespace KrolStudio
{
    public class SettingsPersistor : PlayerPrefsPersistor<SettingsData>
    {
        private readonly SignalBus _signalBus;

        public SettingsPersistor(SignalBus signalBus, DefaultProgressConfig def)
            : base("game_settings", new SettingsData(def.vibrate, def.musicVolume, def.sfxVolume))
        {
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            base.Initialize();

            _signalBus.Subscribe<SettingsChangedSignal>(OnChanged);
            _signalBus.Subscribe<RequestSettingsSignal>(OnRequested);
            _signalBus.Subscribe<ResetSaveDataSignal>(OnReset);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<SettingsChangedSignal>(OnChanged);
            _signalBus.Unsubscribe<RequestSettingsSignal>(OnRequested);
            _signalBus.Unsubscribe<ResetSaveDataSignal>(OnReset);

            base.Dispose();
        }

        private void OnReset(ResetSaveDataSignal _)
        {
            _signalBus.Fire(new SettingsLoadedSignal
            {
                MusicVolume = _defaultData.MusicVolume,
                SfxVolume = _defaultData.SfxVolume,
                Vibrate = _defaultData.Vibrate
            });
        }

        private void OnRequested() =>
            _signalBus.Fire(new SettingsLoadedSignal
            {
                MusicVolume = _dataModel.MusicVolume,
                SfxVolume = _dataModel.SfxVolume,
                Vibrate = _dataModel.Vibrate
            });

        private void OnChanged(SettingsChangedSignal signal)
        {
            _dataModel.MusicVolume = signal.MusicVolume;
            _dataModel.SfxVolume = signal.SfxVolume;
            _dataModel.Vibrate = signal.Vibrate;

            base.SaveData();
        }
    }
}
