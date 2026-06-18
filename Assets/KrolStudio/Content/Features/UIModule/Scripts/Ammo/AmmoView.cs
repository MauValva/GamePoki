using TMPro;
using UnityEngine;

namespace KrolStudio
{
    public class AmmoView : MonoBehaviour, IDisplayable
    {
        [SerializeField] TextMeshProUGUI ammoText;
        [SerializeField] Animator animator;
        [SerializeField] GameObject warningImage;
        [SerializeField] PopUpText outOfBulletsPrefab;

        readonly int ScaleBullets = Animator.StringToHash(GameConstants.Animations.ScaleBullets);
        readonly int ScaleRedBullets = Animator.StringToHash(GameConstants.Animations.ScaleRedBullets);

        public void UpdateAmmoUI(int currentBullets, int lowBulletsThreshold)
        {
            warningImage.SetActive(currentBullets > 0 && currentBullets <= lowBulletsThreshold);
            ammoText.text = currentBullets.ToString();
            animator.SetTrigger(currentBullets > 0 ? ScaleBullets : ScaleRedBullets);
        }

        public void ShowOutOfBulletsMessage() =>
            ShowPopUpMessage(outOfBulletsPrefab);

        Object ShowPopUpMessage(PopUpText prefab)
        {
            PopUpText message = Instantiate(prefab, transform);
            message.OnFinalize += () => Destroy(message.gameObject);
            message.Play();
            return message;
        }

        public void Display(bool value) =>
            gameObject.SetActive(value);
    }
}