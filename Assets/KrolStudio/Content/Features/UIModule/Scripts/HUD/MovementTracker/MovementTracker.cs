using Zenject;

namespace KrolStudio
{
    public class MovementTracker
    {
        private readonly MovementServiceModel _movementModel;

        [Inject]
        public MovementTracker(MovementServiceModel movementModel)
        {
            _movementModel = movementModel;
        }

        public float Distance => _movementModel.Value != null 
            ? _movementModel.Value.NormalizedTime * _movementModel.Value.SplineLength
            : 0f;

        public float Progress => _movementModel.Value?.NormalizedTime ?? 0f;
    }
}