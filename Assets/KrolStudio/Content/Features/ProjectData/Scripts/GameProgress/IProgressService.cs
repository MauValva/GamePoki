using System;

namespace KrolStudio
{
    public interface IProgressService
    {
        event Action<int> OnSetLevel;
        event Action<int> OnSetIncomePurchase;
        event Action<int> OnSetPartPurchase;
        event Action<bool> OnSetTutorialShown;
        event Action<bool> OnSetForcedAdsEnabled;

        void SetLevel(int value);
        void SetIncomePurchaseCount(int value);
        void SetPartPurchaseCount(int value);
        void SetTutorialCompleted(bool value);
        void SetForcedAdsEnabled(bool value);

        int GetLevel();
        int GetIncomePurchaseCount();
        int GetPartPurchaseCount();
        bool IsTutorialCompleted();
        bool IsForcedAdsEnabled();
    }
}