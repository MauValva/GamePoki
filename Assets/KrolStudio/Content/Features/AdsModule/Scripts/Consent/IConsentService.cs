using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public interface IConsentService
    {
        bool IsConsentObtained { get; }
        bool IsTrackingAuthorized { get; }

        UniTask RequestConsentAsync();
        UniTask RequestTrackingAsync();
    }
}