using System;
using UnityEngine;

namespace KrolStudio
{
    public class TentacleAnimator : MonoBehaviour, ITentacleAnimator
    {
        readonly int Death = Animator.StringToHash(GameConstants.Animations.TentacleDeath);
        readonly int Attack = Animator.StringToHash(GameConstants.Animations.TentacleAttack);

        Animator animator;
        public Animator Animator
        {
            get
            {
                if (animator == null)
                    animator = gameObject.GetComponent<Animator>();
                return animator;
            }
        }

        Action OnAttackAnimationEnd;

        private void OnEnable() =>
           Enable(true); // when spawning from the pool - turn it off

        public void Enable(bool value) =>
            Animator.enabled = value;

        public void PlayDeath()
        {
            Animator.SetTrigger(Death);
        }

        public void PlayAttack(Action onEndHitAnimation = null)
        {
            if (Animator == null) return;
            OnAttackAnimationEnd = onEndHitAnimation;
            Animator.SetTrigger(Attack);
        }

        public void OnExitAttackState() // Animator`s event
        {
            if (Animator == null) return;
            OnAttackAnimationEnd.Invoke();
        }
    }
}