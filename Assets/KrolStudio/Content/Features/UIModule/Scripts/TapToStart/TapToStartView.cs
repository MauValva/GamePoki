using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    public class TapToStartView : UIScreen
    {
        [SerializeField] Button closeButton;

        public event Action CloseAction;

        private void Awake()
        {
            closeButton.onClick.AddListener(() => CloseAction?.Invoke());
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
        }

        public void OnClose() =>
           CloseAction?.Invoke();

        public override UniTask Hide()
        {
            gameObject.SetActive(false);
            return default;
        }

        public override UniTask Show()
        {
            gameObject.SetActive(true);
            return default;
        }
    }
}