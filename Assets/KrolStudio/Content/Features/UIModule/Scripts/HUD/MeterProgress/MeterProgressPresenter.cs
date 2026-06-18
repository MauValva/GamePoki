using System;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    // Presenter — subscribes to the event, updates the View
    public class MeterProgressPresenter : IInitializable
    {
        private readonly MeterProgress _view;
        private readonly MovementTracker _tracker;
        private readonly MovementServiceModel _movementModel;
        private readonly DisplayablesModel _model;

        private IMovementService _movement;

        [Inject]
        public MeterProgressPresenter(
            MeterProgress view,
            MovementTracker tracker,
            MovementServiceModel movementModel,
            DisplayablesModel model)
        {
            _view = view;
            _tracker = tracker;
            _movementModel = movementModel;
            _model = model;
        }

        public void Initialize()
        {
            _movementModel.OnServiceRegistered += OnServiceRegistered;
            _model.Register(_view);
        }

        public void Dispose()
        {
            _movementModel.OnServiceRegistered -= OnServiceRegistered;
            _model.Unregister(_view);

            if (_movement != null)
                _movement.OnProgressChanged -= OnProgressChanged;
        }

        private void OnServiceRegistered(IMovementService movement)
        {
            _movement = movement;
            _movement.OnProgressChanged += OnProgressChanged;
        }

        private void OnProgressChanged(float normalizedTime)
        {
            _view.SetProgressView(_tracker.Progress);
            _view.SetTraveledDistance(Mathf.FloorToInt(_tracker.Distance));
        }
    }
}