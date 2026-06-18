using Zenject;

namespace KrolStudio
{
    public class ProgressPersistor : PlayerPrefsPersistor<ProgressData>
    {
        private readonly SignalBus _signalBus;

        public ProgressPersistor(SignalBus signalBus, DefaultProgressConfig defaultProgress)
            : base("game_progress", new ProgressData())
        {
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            base.Initialize();

            _signalBus.Subscribe<ProgressDataChangedSignal>(OnChanged);
            _signalBus.Subscribe<RequestProgressDataSignal>(OnRequested);
            _signalBus.Subscribe<ResetSaveDataSignal>(OnReset);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<ProgressDataChangedSignal>(OnChanged);
            _signalBus.Unsubscribe<RequestProgressDataSignal>(OnRequested);
            _signalBus.Unsubscribe<ResetSaveDataSignal>(OnReset);

            base.Dispose();
        }

        private void OnReset(ResetSaveDataSignal _)
        {
            _signalBus.Fire(new ProgressDataLoadedSignal       
            {
                CurrentLevel = _defaultData.CurrentLevel,
                IncomePurchaseCount = _defaultData.IncomePurchaseCount,
                PartPurchaseCount = _defaultData.PartPurchaseCount,
                IsTutorialCompleted = _defaultData.IsTutorialShown,
                IsForcedAdEnabled = _defaultData.IsForcedAdEnabled
            });
        }

        private void OnRequested() =>
            _signalBus.Fire(new ProgressDataLoadedSignal
            {
                CurrentLevel = _dataModel.CurrentLevel,
                IncomePurchaseCount = _dataModel.IncomePurchaseCount,
                PartPurchaseCount = _dataModel.PartPurchaseCount,
                IsTutorialCompleted = _dataModel.IsTutorialShown,
                IsForcedAdEnabled = _dataModel.IsForcedAdEnabled
            });

        private void OnChanged(ProgressDataChangedSignal signal)
        {
            if (signal.Flags.HasFlag(ProgressDataFlags.CurrentLevel))
                _dataModel.CurrentLevel = signal.CurrentLevel;
            
            if (signal.Flags.HasFlag(ProgressDataFlags.IncomePurchaseCount))
                _dataModel.IncomePurchaseCount = signal.IncomePurchaseCount;
            
            if (signal.Flags.HasFlag(ProgressDataFlags.PartPurchaseCount))
                _dataModel.PartPurchaseCount = signal.PartPurchaseCount;
           
            if (signal.Flags.HasFlag(ProgressDataFlags.IsTutorialShown))
                _dataModel.IsTutorialShown = signal.IsTutorialCompleted;
            
            if (signal.Flags.HasFlag(ProgressDataFlags.IsForcedAdEnabled))
                _dataModel.IsForcedAdEnabled = signal.IsForcedAdEnabled;

            base.SaveData();
        }
    }
}