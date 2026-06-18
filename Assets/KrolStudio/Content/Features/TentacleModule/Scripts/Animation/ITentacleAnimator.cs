using System;

namespace KrolStudio
{
    public interface ITentacleAnimator
    {
        void Enable(bool value);
        void PlayDeath();
        void PlayAttack(Action onEndHitAnimation = null);
        void OnExitAttackState();
    }
}