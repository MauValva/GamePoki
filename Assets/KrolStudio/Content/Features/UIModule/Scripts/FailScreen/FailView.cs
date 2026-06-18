using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    public class FailView : UIScreen
    {
        [SerializeField] Button restartButton;

        public event Action RestartAction;

        private void Awake()
        {
            restartButton.onClick.AddListener(() => RestartAction?.Invoke());
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveAllListeners();
        }

        public void OnNext() =>
           RestartAction?.Invoke();

        public override async UniTask Show()
        {
            gameObject.SetActive(true);
        }

        public override async UniTask Hide()
        {
            gameObject.SetActive(false);
        }
    }
}