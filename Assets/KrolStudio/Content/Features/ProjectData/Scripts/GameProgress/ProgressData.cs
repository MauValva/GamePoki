using System;

namespace KrolStudio
{
    [Serializable]
    public class ProgressData : IDataModel
    {
        public int CurrentLevel = 0;
        public int IncomePurchaseCount = 1;
        public int PartPurchaseCount = 1;
        public bool IsTutorialShown = false;
        public bool IsForcedAdEnabled = false;
    }
}