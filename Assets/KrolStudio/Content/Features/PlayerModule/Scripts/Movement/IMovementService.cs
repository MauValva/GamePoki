using System;

namespace KrolStudio
{
    public interface IMovementService
    {
        event Action<float> OnProgressChanged;
        event Action SplineMoveStarted;
        event Action SplineMoveCompleted;

        float SplineLength { get; }
        float NormalizedTime { get; }

        void SetSpeed(float speed);
        void Tick();
        void Restart();
        bool HasReachedPoint(float destination, float threshold);
    }
}