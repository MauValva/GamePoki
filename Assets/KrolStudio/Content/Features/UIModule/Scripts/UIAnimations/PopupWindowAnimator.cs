using DG.Tweening;
using System;
using UnityEngine;

namespace KrolStudio
{
    public class PopupWindowAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform _window;
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _offsetY = 300f;

        public event Action OnShowStart;
        public event Action OnHideStart;
        public event Action OnShowComplete;
        public event Action OnHideComplete;

        private Vector2 _targetPosition;
        private Sequence _sequence;

        public bool IsAnimating => _sequence != null && _sequence.IsActive();

        private void Awake()
        {
            _targetPosition = _window.anchoredPosition;

            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }

        public void Show()
        {
            _sequence?.Kill();

            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            _window.anchoredPosition = _targetPosition + Vector2.up * _offsetY;

            OnShowStart?.Invoke();

            _sequence = DOTween.Sequence();

            _sequence.Join(_window.DOAnchorPos(_targetPosition, _duration).SetEase(Ease.OutBack));
            _sequence.Join(_canvasGroup.DOFade(1f, _duration));

            _sequence.OnComplete(() =>
            {
                OnShowComplete?.Invoke();
            });
        }

        public void HideAndDeactivate()
        {
            _sequence?.Kill();

            OnHideStart?.Invoke();

            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            _sequence = DOTween.Sequence();

            _sequence.Join(_window.DOAnchorPos(_targetPosition + Vector2.up * _offsetY, _duration).SetEase(Ease.InBack));
            _sequence.Join(_canvasGroup.DOFade(0f, _duration));

            _sequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
                OnHideComplete?.Invoke();
            });
        }
    }
}