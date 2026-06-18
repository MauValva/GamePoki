using System;
using UnityEngine;

namespace KrolStudio
{
    public class EnemyAnimator : MonoBehaviour, IEnemyAnimator
    {
        private readonly int Movement = Animator.StringToHash(GameConstants.Animations.EnemyMovement);
        private readonly int MovementVal = Animator.StringToHash(GameConstants.Animations.EnemyMovementVal);
        private readonly int Hit = Animator.StringToHash(GameConstants.Animations.EnemyHit);
        private readonly int Attack = Animator.StringToHash(GameConstants.Animations.EnemyAttack);

        private Action OnHitAnimationEnd;
        private Action OnBehindAttackAnimationEnd;

        private Animator _animator;
        public Animator Animator
        {
            get
            {
                if (_animator == null)
                    _animator = GetComponent<Animator>();
                return _animator;
            }
        }

        private void OnEnable() => 
            Enable(true); // when spawning from the pool - turn it off

        public void Enable(bool value) =>
            Animator.enabled = value;

        public void PlayMovement(bool value)
        {
            if (Animator == null) return;
            Animator.SetBool(Movement, value);
        }

        public void ChangingMovement(float targetValue, float accelerationMultiplier)
        {
            float current = Animator.GetFloat(MovementVal);
            if (Mathf.Approximately(current, targetValue)) return;

            Animator.SetFloat(MovementVal, Mathf.MoveTowards(
                current,
                targetValue,
                accelerationMultiplier * Time.deltaTime));
        }

        public void PlayBehindAttack(Action onEndHitAnimation = null)
        {
            if (Animator == null) return;
            OnBehindAttackAnimationEnd = onEndHitAnimation;
            Animator.SetTrigger(Attack);
        }

        public void PlayHit(Action onEndHitAnimation = null)
        {
            if (Animator == null) return;
            this.OnHitAnimationEnd = onEndHitAnimation;
            Animator.SetTrigger(Hit);
        }

        public void OnExitHitState() // Animator`s event
        {
            if (Animator == null) return;
            OnHitAnimationEnd?.Invoke();
        }

        public void OnExitBehindAttackState() // Animator`s event
        {
            if (Animator == null) return;
            OnBehindAttackAnimationEnd?.Invoke();
        }
    }
}