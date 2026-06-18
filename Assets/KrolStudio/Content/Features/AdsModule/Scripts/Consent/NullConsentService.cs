using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public class NullConsentService : IConsentService
    {
        public bool IsConsentObtained => true;
        public bool IsTrackingAuthorized => true;

        public UniTask RequestConsentAsync() => UniTask.CompletedTask;
        public UniTask RequestTrackingAsync() => UniTask.CompletedTask;
    }
}