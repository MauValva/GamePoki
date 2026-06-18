

namespace KrolStudio
{
    public enum ProgressDataFlags
    {
        CurrentLevel = 1 << 0, // 1
        IncomePurchaseCount = 1 << 1, // 2
        PartPurchaseCount = 1 << 2, // 4
        IsTutorialShown = 1 << 3, // 8
        IsForcedAdEnabled = 1 << 4, // 16
    }

    public struct ProgressDataChangedSignal
    {
        public ProgressDataFlags Flags;
        public int CurrentLevel;
        public int IncomePurchaseCount;
        public int PartPurchaseCount;
        public bool IsTutorialCompleted;
        public bool IsForcedAdEnabled;
    }
}