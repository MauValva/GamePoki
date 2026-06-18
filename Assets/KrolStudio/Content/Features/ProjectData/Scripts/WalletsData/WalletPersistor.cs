using Zenject;

namespace KrolStudio
{
    public class WalletPersistor : PlayerPrefsPersistor<WalletData>
    {
        private readonly SignalBus _signalBus;

        public WalletPersistor(SignalBus signalBus, DefaultProgressConfig defaultProgress)
            : base("wallet_data", new WalletData(defaultProgress.currencySettings))
        {
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            base.Initialize();

            _signalBus.Subscribe<WalletChangedSignal>(OnChanged);
            _signalBus.Subscribe<RequestWalletSignal>(OnRequested);
            _signalBus.Subscribe<ResetSaveDataSignal>(OnReset);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<WalletChangedSignal>(OnChanged);
            _signalBus.Unsubscribe<RequestWalletSignal>(OnRequested);
            _signalBus.Unsubscribe<ResetSaveDataSignal>(OnReset);

            base.Dispose();
        }

        private void OnReset(ResetSaveDataSignal _)
        {
            _signalBus.Fire(new WalletLoadedSignal
            {
                Entries = new(_defaultData.Entries)
            });
        }

        private void OnRequested() =>
            _signalBus.Fire(new WalletLoadedSignal
            {
                Entries = new(_dataModel.Entries) 
            });

        private void OnChanged(WalletChangedSignal signal)
        {
            _dataModel.Entries = new(signal.Entries);
            base.SaveData();
        }
    }
}