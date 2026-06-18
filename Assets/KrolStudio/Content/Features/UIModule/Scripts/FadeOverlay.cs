using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KrolStudio
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeOverlay : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _duration = 0.5f;

        private Sequence _sequence;

        public async UniTask FadeInAsync()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _canvasGroup.blocksRaycasts = true;

            _sequence.Join(_canvasGroup.DOFade(1f, _duration));

            await _sequence.AsyncWaitForCompletion().AsUniTask();
        }

        public async UniTask FadeOutAsync()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _canvasGroup.blocksRaycasts = false;

            _sequence.Join(_canvasGroup.DOFade(0f, _duration));

            await _sequence.AsyncWaitForCompletion().AsUniTask();
        }
    }
}