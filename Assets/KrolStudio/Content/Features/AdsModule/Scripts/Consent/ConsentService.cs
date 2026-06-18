#if ADMOB
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace KrolStudio
{
    public class ConsentService : IConsentService
    {
        public bool IsConsentObtained { get; private set; }
        public bool IsTrackingAuthorized { get; private set; }

        public async UniTask RequestConsentAsync()
        {
#if UNITY_IOS || UNITY_ANDROID
            var tcs = new UniTaskCompletionSource();

            ConsentInformation.Update(new ConsentRequestParameters(), error =>
            {
                if (error != null)
                {
                    Debug.LogWarning($"[Consent] Update error: {error.Message}");
                    IsConsentObtained = false;
                    tcs.TrySetResult();
                    return;
                }

                var status = ConsentInformation.ConsentStatus;

                if (status == ConsentStatus.NotRequired || status == ConsentStatus.Obtained)
                {
                    IsConsentObtained = true;
                    tcs.TrySetResult();
                    return;
                }

                if (!ConsentInformation.IsConsentFormAvailable())
                {
                    IsConsentObtained = false;
                    tcs.TrySetResult();
                    return;
                }

                ConsentForm.Load((form, loadError) =>
                {
                    if (loadError != null)
                    {
                        Debug.LogWarning($"[Consent] Form load error: {loadError.Message}");
                        tcs.TrySetResult();
                        return;
                    }

                    form.Show(showError =>
                    {
                        if (showError != null)
                            Debug.LogWarning($"[Consent] Form show error: {showError.Message}");

                        IsConsentObtained =
                            ConsentInformation.ConsentStatus == ConsentStatus.Obtained ||
                            ConsentInformation.ConsentStatus == ConsentStatus.NotRequired;

                        tcs.TrySetResult();
                    });
                });
            });

            await tcs.Task;
#else
            IsConsentObtained = true;
            await UniTask.CompletedTask;
#endif
        }

        public async UniTask RequestTrackingAsync()
        {
#if UNITY_IOS
            var status = await Unity.Advertisement.IosSupport
                .ATTrackingStatusBinding.RequestAuthorizationTrackingAsync();

            IsTrackingAuthorized =
                status == Unity.Advertisement.IosSupport
                    .ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED;

            Debug.Log($"[IDFA] Tracking status: {status}");
#else
            IsTrackingAuthorized = true;
            await UniTask.CompletedTask;
#endif
        }
    }
}
#endif