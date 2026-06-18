
namespace KrolStudio
{
    public interface INoAdsService
    {
        bool IsPurchased { get; }
        void SetPurchased();
    }
}