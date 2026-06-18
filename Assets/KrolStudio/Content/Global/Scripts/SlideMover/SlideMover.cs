using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [Serializable]
    class SlideInfo
    {
        public Transform thisTransform;
        public Vector3 offset = new Vector3(0, -5f, 0);
        public float duration = 1f;
    }

    public class SlideMover : MonoBehaviour
    {
        [SerializeField] SlideInfo list;

        private CtsHelper ctsHelper = new();

        [Inject]
        void Construct(SlideMoverContext moverContext)
        {
            moverContext.RegisterRoot(this);
        }

        public async UniTask StartSlide()
        {
            await SlideOut(list);
        }

        private async UniTask SlideOutAndTrack(SlideInfo obj, System.Action onComplete)
        {
            await SlideOut(obj);
            onComplete?.Invoke();
        }

        private async UniTask SlideOut(SlideInfo info)
        {
            try
            {
                float time = 0f;

                Vector3 startPos = info.thisTransform.localPosition;
                Vector3 worldOffset = info.thisTransform.TransformDirection(info.offset);
                Vector3 targetPos = startPos + worldOffset;

                while (time < info.duration)
                {
                    float t = time / info.duration;
                    info.thisTransform.localPosition = Vector3.Lerp(startPos, targetPos, Easing.EaseInQuint(t));
                    time += Time.deltaTime;
                    await UniTask.NextFrame(ctsHelper.Token);
                }

                info.thisTransform.localPosition = targetPos;
                info.thisTransform.gameObject.SetActive(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (MissingReferenceException ex)
            {
                Debug.LogWarning($"The object was destroyed during animation: {ex.Message}", this);
            }
            catch (NullReferenceException ex)
            {
                Debug.LogWarning($"Missing link during animation: {ex.Message}", this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error: {ex}", this);
            }
        }

        private void OnDestroy()
        {
            ctsHelper.Dispose();
        }
    }
}