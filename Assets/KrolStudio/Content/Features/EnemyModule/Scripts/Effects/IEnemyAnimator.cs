using System;

namespace KrolStudio
{
    public interface IEnemyAnimator
    {
        void Enable(bool value);
        void PlayMovement(bool value);
        void ChangingMovement(float targetValue, float accelerationMultiplier);
        void PlayBehindAttack(Action onEndHitAnimation = null);
        void PlayHit(Action onEndHitAnimation = null);
    }
}