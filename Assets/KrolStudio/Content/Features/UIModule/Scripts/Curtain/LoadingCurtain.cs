using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] float fadeInDuration = 1f;
        [SerializeField] float fadeOutDuration = 1f;
        [SerializeField] float dotsFadeOutDuration = 0.35f;
        [SerializeField] CanvasGroup cg;
        [SerializeField] Image dotsBackground;

        private float alphaControl;
        private Material dotsMaterial;

        private Sequence _sequence;

        private void Awake()
        {
            dotsMaterial = new Material(dotsBackground.material);
            dotsBackground.material = dotsMaterial;
            alphaControl = dotsMaterial.GetFloat("_AlphaControl");
            cg.alpha = 0f;
            cg.interactable = false;
        }

        public async UniTask FadeInAsync()
        {
            cg.blocksRaycasts = true;

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
           
            _sequence.Join(cg.DOFade(1f, 0.1f));
            
            _sequence.Join(DOTween.To(() => 0f, x => dotsMaterial.SetFloat("_AlphaControl", x), alphaControl, 0.1f).SetEase(Ease.InCubic));
            
            _sequence.OnComplete(Show);

            await _sequence.AsyncWaitForCompletion().AsUniTask();
        }

        public async UniTask FadeOutAsync()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.AppendInterval(1.5f);

            _sequence.Append(cg.DOFade(0f, fadeOutDuration).SetEase(Ease.InCubic));

            _sequence.Join(
                DOTween.To(() => alphaControl, x => dotsMaterial.SetFloat("_AlphaControl", x), 0f, dotsFadeOutDuration).SetEase(Ease.InCubic)
            );

            _sequence.OnComplete(() =>
            {
                cg.alpha = 0f;
                cg.blocksRaycasts = false;
                dotsMaterial.SetFloat("_AlphaControl", 0f);
            });

            await _sequence.AsyncWaitForCompletion().AsUniTask();
        }


        public void Show()
        {         
            dotsMaterial.SetFloat("_AlphaControl", alphaControl);
        }

        private void OnDestroy()
        {
            if (dotsMaterial != null)
            {
                Destroy(dotsMaterial);
                dotsMaterial = null;
            }
        }
    }
}