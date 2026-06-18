using System.Collections.Generic;
using Zenject;

namespace KrolStudio
{
    public class PlayerPersistor : PlayerPrefsPersistor<PlayerData>
    {
        private readonly SignalBus _signalBus;

        public PlayerPersistor(SignalBus signalBus, DefaultProgressConfig defaultProgress)
            : base("player_data", new PlayerData(defaultProgress.partSettings))
        {
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            base.Initialize();

            _signalBus.Subscribe<PlayerDataChangedSignal>(OnChanged);
            _signalBus.Subscribe<RequestPlayerDataSignal>(OnRequested);
            _signalBus.Subscribe<ResetSaveDataSignal>(OnReset);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDataChangedSignal>(OnChanged);
            _signalBus.Unsubscribe<RequestPlayerDataSignal>(OnRequested);
            _signalBus.Unsubscribe<ResetSaveDataSignal>(OnReset);

            base.Dispose();
        }

        private void OnReset(ResetSaveDataSignal _)
        {
            _signalBus.Fire(new PlayerDataLoadedSignal
            {
                Entries = new(_defaultData.Entries)
            });
        }

        private void OnRequested() =>
          _signalBus.Fire(new PlayerDataLoadedSignal
          {
              Entries = new(_dataModel.Entries)
          });

        private void OnChanged(PlayerDataChangedSignal signal)
        {
            _dataModel.Entries = new(signal.Entries);
            base.SaveData();
        }
    }
}