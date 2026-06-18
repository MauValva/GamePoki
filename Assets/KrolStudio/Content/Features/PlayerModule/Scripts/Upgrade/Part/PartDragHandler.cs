using System;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    // PartDragHandler — updating ReturnToNearestCell and MoveToGarbage
    public class PartDragHandler : MonoBehaviour
    {
        public event Action PointerDown;
        public event Action PointerUp;
        public event Action PointerDrag;

        private DragTarget _dragTarget;
        private PartLifter _partLifter;
        private CurveMover _curveMover;

        private IBoardInteractionService _boardInteraction;
        private IGarbageService _garbageService;

        [Inject]
        private void Construct(
            IBoardInteractionService boardInteraction,
            IGarbageService garbageService)
        {
            _boardInteraction = boardInteraction;
            _garbageService = garbageService;
        }

        private void Awake()
        {
            _partLifter = GetComponent<PartLifter>();
            _dragTarget = GetComponent<DragTarget>();
            _curveMover = GetComponent<CurveMover>();
        }

        private void OnEnable()
        {
            _dragTarget.PointerDown += OnPointerDownInternal;
            _dragTarget.PointerUp += OnPointerUpInternal;
            _dragTarget.PointerDrag += OnPointerDragInternal;
        }

        private void OnDisable()
        {
            _dragTarget.PointerDown -= OnPointerDownInternal;
            _dragTarget.PointerUp -= OnPointerUpInternal;
            _dragTarget.PointerDrag -= OnPointerDragInternal;
        }

        private void OnPointerDownInternal()
        {
            _curveMover.Cancel();
            _partLifter.StartLifting();
            PointerDown?.Invoke();
        }

        private void OnPointerUpInternal()
        {
            _partLifter.CancelLifting();
            PointerUp?.Invoke();
        }

        private void OnPointerDragInternal() =>
            PointerDrag?.Invoke();

        public void ReturnToNearestCell()
        {
            var cell = _boardInteraction.GetNearestFreeCell(transform.position);
            if (cell == null) return;

            transform.SetParent(cell);
            _curveMover.MoveTo(cell, Easing.EaseInQuart, ResetPointerFlags);
        }

        public void MoveToGarbage(Action onComplete)
        {
            var garbageTransform = _garbageService.GarbageTransform;
            transform.SetParent(null);
            _curveMover.MoveTo(garbageTransform, Easing.EaseInQuart, onComplete);
            _curveMover.ScaleTo(Vector3.zero, Easing.EaseInQuart);
        }

        private void ResetPointerFlags()
        {
            _dragTarget.IsPointerDown = false;
            _dragTarget.IsPointerUp = false;
        }
    }
}