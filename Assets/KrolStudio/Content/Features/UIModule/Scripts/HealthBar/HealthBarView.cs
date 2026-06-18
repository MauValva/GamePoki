using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    public class HealthBarView : MonoBehaviour, IDisplayable
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Image delayedFillImage;
        [SerializeField] private float reduceSpeed = 0.3f;

        private float _maxHealth;

        private Tween delayedTween;

        public void Display(bool value) =>
            gameObject.SetActive(value);

        public void SetMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void UpdateHealth(float currentHealth)
        {
            fillImage.fillAmount = Mathf.Clamp01(currentHealth / _maxHealth);

            AnimateDelayedBar();
        }

        private void AnimateDelayedBar()
        {
            delayedTween?.Kill();

            delayedTween = delayedFillImage
                .DOFillAmount(fillImage.fillAmount, reduceSpeed)
                .SetEase(Ease.InQuint);
        }
    }
}