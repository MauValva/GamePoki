using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelIndicator;
        [SerializeField] TextMeshProUGUI coinsAmount;
        [SerializeField] Button pauseButton;

        public event Action PauseAction;

        private void Awake()
        {
            pauseButton.onClick.AddListener(() => PauseAction?.Invoke());
        }

        private void OnDestroy()
        {
            pauseButton.onClick.RemoveAllListeners();
        }

        public void SetPausaButtonInteractable(bool value) =>
            pauseButton.interactable = value;

        public void SetMoney(int money) =>
          coinsAmount.text = money.ToString("N0", CultureInfo.InvariantCulture);

        public void SetLevel(int level) =>
            levelIndicator.text = $"LEVEL {level}";
    }
}