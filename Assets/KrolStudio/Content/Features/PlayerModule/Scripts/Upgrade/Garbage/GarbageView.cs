using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;
using Core.InputModule;

namespace KrolStudio
{
    public class GarbageView : MonoBehaviour, IGarbageService
    {
        [SerializeField] Transform garbage;
        [SerializeField] SpriteRenderer background;
        [SerializeField] Color readyColor;
        [SerializeField] Color normalColor;
        [SerializeField] float duration;

        private CtsHelper _ctsHelper = new();
        private IInputListener _input;

        public Transform GarbageTransform => transform;

        public bool IsHovered =>
            RaycastUtility.RaycastFromPosition(out _, _input.CurrentMousePosition, LayerMask.GetMask("Garbage"));

        [Inject]
        private void Construct(IInputListener input)
        {
            _input = input;
        }

        private void Awake()
        {
            garbage.localScale = Vector3.zero;
        }

        public void Show()
        {
            background.color = normalColor;
            AnimateScale(0f, 1f).Forget();
        }

        public void Hide() =>
            AnimateScale(1f, 0f).Forget();

        public void UpdateHoverColor() =>
            background.color = IsHovered ? readyColor : normalColor;

        private async UniTask AnimateScale(float from, float to)
        {
            _ctsHelper.Restart();
            try
            {
                float time = 0f;
                while (time < duration)
                {
                    float t = time / duration;
                    garbage.localScale = Vector3.one * Mathf.Lerp(from, to, Easing.EaseOutQuart(t));
                    time += Time.deltaTime;
                    await UniTask.NextFrame(_ctsHelper.Token);
                }
                garbage.localScale = Vector3.one * to;
            }
            catch (OperationCanceledException) { }
        }
    }
}