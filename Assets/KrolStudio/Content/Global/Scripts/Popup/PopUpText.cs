using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace KrolStudio
{
    public class PopUpText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _popupText;
        [SerializeField] private CanvasGroup _cg;
        [Space]
        [SerializeField] private float _moveDuration;
        [SerializeField] private float _moveDistance;
        [SerializeField] private float _startDelay;
        [Space]
        [SerializeField][Range(0.0001f, 1f)] private float _fadeInEndTime = 0.15f;
        [SerializeField][Range(0f, 1f)] private float _fadeOutStartTime = 0.7f;
        [SerializeField] private AnimationCurve _moveCurve;
        [Space]
        [SerializeField] private float _timeToDestruction;
        [Space]
        [SerializeField] private UnityEvent _onStartAnimation;
        [SerializeField] private UnityEvent _onEndAnimation;

        public event Action OnFinalize;

        private Sequence _sequence;

        public void Play(string withText)
        {
            _popupText.text = withText;
            Play();
        }

        public void Play()
        {
            _sequence?.Kill();
            _sequence = BuildSequence();
            _sequence.Play();
        }

        private Sequence BuildSequence()
        {
            Vector3 startPos = transform.localPosition;
            Vector3 endPos = startPos + Vector3.up * _moveDistance;

            _cg.alpha = 0f;
            transform.localPosition = startPos;

            float fadeInDuration = _fadeInEndTime * _moveDuration;
            float fadeOutStart = _fadeOutStartTime * _moveDuration;
            float fadeOutDuration = _moveDuration - fadeOutStart;

            Sequence sequence = DOTween.Sequence();

            sequence.AppendInterval(_startDelay);                                               // Delay before start
            sequence.AppendCallback(() => _onStartAnimation.Invoke());                          // Start animation
            sequence.Append(transform.DOLocalMove(endPos, _moveDuration).SetEase(_moveCurve));  // Move along the curve
            sequence.Join(_cg.DOFade(1f, fadeInDuration).SetEase(Ease.Linear));                 // Fade in — in parallel with movement
            sequence.Insert(_startDelay + fadeOutStart,_cg.DOFade(0f, fadeOutDuration).SetEase(Ease.Linear));   // Fade out — after _fadeOutStartTime
            sequence.AppendInterval(_timeToDestruction);  // Delay before finalization

            // Final
            sequence.AppendCallback(() =>
            {
                OnFinalize?.Invoke();
                _onEndAnimation.Invoke();
            });

            return sequence;
        }

        private void OnDestroy() =>
            _sequence?.Kill();
    }
}