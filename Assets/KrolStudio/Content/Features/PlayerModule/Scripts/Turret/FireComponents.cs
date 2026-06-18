using UnityEngine;

namespace KrolStudio
{
    [System.Serializable]
    public class FireComponents
    {
        [SerializeField] Transform firePoint;
        [SerializeField] ParticleSystem fireEffect;
        [SerializeField] Animator fireAnimator;

        [SerializeField] string fireAnimationKey = "Fire";

        int animationHash;

        public int AnimationHash()
        {
            if (animationHash == 0)
                animationHash = Animator.StringToHash(fireAnimationKey);
            return animationHash;
        }

        public Transform FirePoint => firePoint;

        public void PlayFireFx() =>
            fireEffect.Play();

        public void PlayFireAnime() =>
            fireAnimator.SetTrigger(AnimationHash());
    }
}