
namespace KrolStudio
{
    public interface IWalletService : IStorageService<CurrencyType>
    {
        // Wallet-specific — for example, a purchase.
        bool TryPurchase(CurrencyType currency, int price, object sender);
    }
}
