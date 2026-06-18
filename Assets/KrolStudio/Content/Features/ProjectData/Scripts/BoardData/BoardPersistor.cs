using Zenject;

namespace KrolStudio
{
    public class BoardPersistor : PlayerPrefsPersistor<BoardData>
    {
        private readonly SignalBus _signalBus;

        public BoardPersistor(SignalBus signalBus, DefaultProgressConfig defaultProgress)
            : base("board_data", new BoardData())
        {
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            base.Initialize();

            _signalBus.Subscribe<BoardDataChangedSignal>(OnChanged);
            _signalBus.Subscribe<RequestBoardDataSignal>(OnRequested);
            _signalBus.Subscribe<ResetSaveDataSignal>(OnReset);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<BoardDataChangedSignal>(OnChanged);
            _signalBus.Unsubscribe<RequestBoardDataSignal>(OnRequested);
            _signalBus.Unsubscribe<ResetSaveDataSignal>(OnReset);

            base.Dispose();
        }

        private void OnReset(ResetSaveDataSignal _)
        {
           _signalBus.Fire(new BoardDataLoadedSignal
            {
                Entries = new()
            });
        }

        private void OnRequested() =>
             _signalBus.Fire(new BoardDataLoadedSignal
             {
                 Entries = new(_dataModel.Entries)
             });

        private void OnChanged(BoardDataChangedSignal signal)
        {
            _dataModel.Entries = new(signal.Entries);
            base.SaveData();
        }
    }
}