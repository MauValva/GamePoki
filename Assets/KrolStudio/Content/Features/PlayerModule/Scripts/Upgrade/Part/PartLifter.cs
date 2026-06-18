using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace KrolStudio
{
    public class PartLifter : MonoBehaviour
    {
        [SerializeField] float duration = 0.1f;

        private CtsHelper ctsHelper = new();

        public void StartLifting()
        {
            transform.SetParent(null);

            CancelLifting();   // If it's already running — restart it

            LiftPartCoroutine().Forget();
        }

        public void CancelLifting()
        {
            ctsHelper.Restart();
        }

        private async UniTask LiftPartCoroutine()
        {
            try
            {
                // Change only Y to avoid conflicts with DragTarget
                float startY = transform.position.y;
                float endY = startY + 1f;
                float time = 0f;

                while (time < duration)
                {
                    float t = time / duration;
                    float easedY = Mathf.Lerp(startY, endY, Easing.EaseOutQuart(t));
                    transform.position = new Vector3(transform.position.x, easedY, transform.position.z);

                    time += Time.deltaTime;
                    await UniTask.NextFrame(ctsHelper.Token);
                }

                transform.position = new Vector3(transform.position.x, endY, transform.position.z);
            }
            catch (OperationCanceledException)
            {
            }
            catch (MissingReferenceException ex)
            {
                Debug.LogWarning($"The object was destroyed during animation: {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                Debug.LogWarning($"Missing link during animation: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error: {ex}");
            }
        }

        private void OnDestroy()
        {
            ctsHelper.Dispose();
        }
    }
}