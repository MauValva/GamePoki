using Cysharp.Threading.Tasks;
using Global.Scripts.Generated;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class BoardService : IInitializable, IDisposable
    {
        private readonly IPrefabsFactory _prefabsFactory;
        private readonly BoardView _boardView;
        private readonly SignalBus _signalBus;
        private readonly BoardServiceModel _boardService;
        private readonly UpgradeLevelController upgradeLevel;

        public bool HasFreeSlot => GetFreeCellIndex() >= 0;

        [Inject]
        public BoardService(
            IPrefabsFactory prefabsFactory,
            BoardView boardView,
            SignalBus signalBus,
            BoardServiceModel boardService,
            UpgradeLevelController upgradeLevel)
        {
            _prefabsFactory = prefabsFactory;
            _boardView = boardView;
            _signalBus = signalBus;
            _boardService = boardService;
            this.upgradeLevel = upgradeLevel;
        }

        public void Initialize()
        {
            _boardService.Value = this;
            _signalBus.Subscribe<BoardDataLoadedSignal>(OnDataLoaded);
            _signalBus.Fire(new RequestBoardDataSignal());
        }

        public void Dispose() =>
            _signalBus.Unsubscribe<BoardDataLoadedSignal>(OnDataLoaded);

        private void OnDataLoaded(BoardDataLoadedSignal signal)
        {
            foreach (var entry in signal.Entries)
            {
                if (entry.cellIndex < 0 || entry.cellIndex >= _boardView.TotalSlots) continue;
                SpawnPart(entry);
            }

            if(signal.Entries.Count == 0)
                ClearBoardView();

            BoardChangedSignal().Forget();
        }

        public void FireChanged() =>
            _signalBus.Fire(new BoardDataChangedSignal
            {
                Entries = _boardView.GetBoardEntry()
            });

        public bool TryAddPart(PartType partType)
        {
            int slotIndex = GetFreeCellIndex();
            if (slotIndex < 0) return false;

            var entry = new BoardEntry
            {
                type = partType,
                level = 0,
                cellIndex = slotIndex
            };

            SpawnPart(entry);
            BoardChangedSignal().Forget();

            _signalBus.Fire(new TutorialStepCompletedSignal { StepId = "AddPart" });
            return true;
        }

        public bool TryAddPart()
        {
            int slotIndex = GetFreeCellIndex();
            if (slotIndex < 0) return false;

            var entry = new BoardEntry
            {
                type = GetRandomPartType(),
                level = 0,
                cellIndex = slotIndex
            };

            SpawnPart(entry);
            BoardChangedSignal().Forget();

            _signalBus.Fire(new TutorialStepCompletedSignal { StepId = "AddPart" });
            return true;
        }

        public void HideLevelIndicators() =>
            upgradeLevel.DisplayLevelIndicators(false);

        private float GetWeight(int level)
        {
            level = level < 0 ? 1 : level + 1;
            return Mathf.Max(_boardView.MinWeight, 1f / Mathf.Pow(level, _boardView.LevelDecayExponent));
        }

        private PartType GetRandomPartType()
        {
            var values = Enum.GetValues(typeof(PartType))
                 .Cast<PartType>()
                 .Where(v => v != PartType.None)
                 .ToArray();

            float[] weights = new float[values.Length];

            for (int i = 0; i < weights.Length; i++)
                weights[i] = GetWeight(upgradeLevel.GetLevel(values[i]));

            float totalWeight = weights.Sum();
            float roll = UnityEngine.Random.value * totalWeight;

            for (int i = 0; i < weights.Length; i++)
            {
                if (roll < weights[i])
                    return values[i];
                roll -= weights[i];
            }

            return values[0]; // fallback — Always deterministic
        }

        public async UniTask BoardChangedSignal()
        {
            await UniTask.NextFrame();
            FireChanged();
            _signalBus.Fire(new BoardChangedSignal());
        }

        private int GetFreeCellIndex() =>
            _boardView.GetFreeCellIndex();

        public PartInteractionHandler SpawnPart(PartType type, int level, Transform cell)
        {
            var newPart = _prefabsFactory.Create<PartInteractionHandler>(Address.Prefabs.Part);
            _boardView.PlacePartInSlot(newPart, cell);
            newPart.Initialize(type, level);
            return newPart;
        }

        private void ClearBoardView() =>
            _boardView.ClearBoardView();

        private void SpawnPart(BoardEntry entry)
        {
            var newPart = _prefabsFactory.Create<PartInteractionHandler>(Address.Prefabs.Part);
            _boardView.PlacePartInSlot(newPart, entry.cellIndex);
            newPart.Initialize(entry.type, entry.level);
        }
    }
}