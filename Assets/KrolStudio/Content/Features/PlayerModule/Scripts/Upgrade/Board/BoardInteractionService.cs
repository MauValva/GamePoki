using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Zenject;
using Core.InputModule;
using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    // IBoardInteractionService — Merge logic on the board.
    public class BoardInteractionService : IBoardInteractionService
    {
        private readonly BoardView _boardView;
        private readonly BoardService _boardService;
        private readonly IPrefabsFactory _prefabsFactory;
        private readonly IInputListener _input;
        private readonly ILogService _logService;
        private readonly SignalBus _signalBus;

        [Inject]
        public BoardInteractionService(
            BoardView boardView,
            IPrefabsFactory prefabsFactory,
            IInputListener input,
            ILogService logService,
            BoardService boardService,
            SignalBus signalBus)
        {
            _boardView = boardView;
            _prefabsFactory = prefabsFactory;
            _input = input;
            _logService = logService;
            _boardService = boardService;
            _signalBus = signalBus;
        }

        public bool TryMerge(PartInteractionHandler dragged, PartType type, int level)
        {
            var results = new List<RaycastResult>();
            if (!RaycastUtility.RaycastFromPosition(out results, _input.CurrentMousePosition, LayerMask.GetMask("Part")))
                return false;

            foreach (var result in results)
            {
                var target = result.gameObject.GetComponent<PartInteractionHandler>();
                if (target == null || target == dragged) continue;
                if (target.PartType != type) continue;
                if (target.Level != level) continue;
                if (!dragged.CanUpgrade) continue;

                MergeInto(dragged, target);

                return true;
            }

            return false;
        }

        private void MergeInto(PartInteractionHandler dragged, PartInteractionHandler target)
        {
            var cell = target.transform.parent;
            int newLevel = dragged.Level + 1;
            var type = dragged.PartType;

            dragged.transform.SetParent(target.transform);
            dragged.GetComponent<CurveMover>().MoveTo(target.transform, Easing.EaseInQuart, () =>
            {
                dragged.DestroySelf();
                target.DestroySelf();
                SpawnMergedPart(cell, type, newLevel);
                _boardService.BoardChangedSignal().Forget(); 
            });
        }



        private void SpawnMergedPart(Transform cell, PartType type, int level)
        {
            var newPart = _boardService.SpawnPart(type, level, cell);
            newPart.GetComponent<PartVisualsController>().PlayEffect();
        }

        public Transform GetNearestFreeCell(Vector3 position) =>
            _boardView.GetNearestFreeCell(position);
    }
}