using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

namespace KrolStudio
{
    public enum UIAnimationDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class UIElementAnimator : MonoBehaviour
    {
        [SerializeField] private UIAnimationDirection _direction;
        [SerializeField] private float _offset = 300f;
        [SerializeField] private float _duration = 0.4f;

        private RectTransform _rect;
        private CanvasGroup _canvas;

        private Vector2 _targetPos;
        private Sequence _sequence;

        public event Action OnShowStart;
        public event Action OnHideStart;
        public event Action OnShowComplete;
        public event Action OnHideComplete;

        bool _isInitialized = false;

        public void Initialize()
        {
            if (_isInitialized is false)
            {
                _canvas = GetComponent<CanvasGroup>();
                _rect = GetComponent<RectTransform>();

                _targetPos = _rect.anchoredPosition;

                _isInitialized = true;
            }
        }

        public async UniTask Show()
        {
            Initialize();

            _sequence?.Kill();

            Vector2 startPos = GetOffsetPosition(_targetPos);

            _rect.anchoredPosition = startPos;
            _canvas.alpha = 0;
            _canvas.blocksRaycasts = true;
            _canvas.interactable = true;

            _sequence = DOTween.Sequence();

            OnShowStart?.Invoke();

            _sequence.Join(_rect.DOAnchorPos(_targetPos, _duration).SetEase(Ease.OutBack));
            _sequence.Join(_canvas.DOFade(1, _duration));

            _sequence.OnComplete(() =>
            {
                OnShowComplete?.Invoke();
            });

            await _sequence.AsyncWaitForCompletion().AsUniTask();
        }

        public async UniTask Hide()
        {
            Initialize();

            _sequence?.Kill();

            OnHideStart?.Invoke();

            Vector2 endPos = GetOffsetPosition(_targetPos);

            _canvas.blocksRaycasts = false;
            _canvas.interactable = false;

            _sequence = DOTween.Sequence();

            _sequence.Join(_rect.DOAnchorPos(endPos, _duration).SetEase(Ease.InBack));
            _sequence.Join(_canvas.DOFade(0, _duration));

            _sequence.OnComplete(() =>
            {
                OnHideComplete?.Invoke();
            });

            await _sequence.AsyncWaitForCompletion().AsUniTask();
        }

        private Vector2 GetOffsetPosition(Vector2 basePos)
        {
            switch (_direction)
            {
                case UIAnimationDirection.Top:
                    return basePos + Vector2.up * _offset;

                case UIAnimationDirection.Bottom:
                    return basePos + Vector2.down * _offset;

                case UIAnimationDirection.Left:
                    return basePos + Vector2.left * _offset;

                case UIAnimationDirection.Right:
                    return basePos + Vector2.right * _offset;

                default:
                    return basePos;
            }
        }
    }
}