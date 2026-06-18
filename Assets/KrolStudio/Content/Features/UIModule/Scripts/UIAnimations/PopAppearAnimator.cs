using System;
using DG.Tweening;
using UnityEngine;

namespace KrolStudio
{
    public class PopAppearAnimator : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _targetTransform;

        [Header("Animation Settings")] 

        [SerializeField] private float _durationIn = 0.25f;
        [SerializeField] private float _durationOut = 0.15f;
        [SerializeField] private float _maxScale = 1.2f;
        [SerializeField] private float _finalScale = 1.0f;

        private Vector3 _hiddenScale = Vector3.zero;
        public event Action OnAppearComplete;
        public event Action OnAppperStart;

        private void Awake()
        {
            if (_targetTransform == null)
                _targetTransform = transform;

            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            _targetTransform.localScale = _hiddenScale;

            if (_canvasGroup != null)
                _canvasGroup.alpha = 0f;
        }

        public void Play()
        {
            _targetTransform.DOKill();
            _canvasGroup?.DOKill();

            OnAppperStart?.Invoke();
            Sequence sequence = DOTween.Sequence();

            _targetTransform.localScale = _hiddenScale;
            if (_canvasGroup != null)
                _canvasGroup.alpha = 0f;

            if (_canvasGroup != null)
                sequence.Append(_canvasGroup.DOFade(1f, _durationIn * 0.5f));

            sequence.Join(_targetTransform.DOScale(_maxScale, _durationIn).SetEase(Ease.OutBack));          // Custom scaling
            sequence.Append(_targetTransform.DOScale(_finalScale, _durationOut).SetEase(Ease.InOutSine));   // Standard scaling

            sequence.OnComplete(() => { OnAppearComplete?.Invoke(); });
        }

        public void HideInstant()
        {
            _targetTransform.DOKill();
            _canvasGroup?.DOKill();

            _targetTransform.localScale = _hiddenScale;
            if (_canvasGroup != null)
                _canvasGroup.alpha = 0f;
        }
    }
}