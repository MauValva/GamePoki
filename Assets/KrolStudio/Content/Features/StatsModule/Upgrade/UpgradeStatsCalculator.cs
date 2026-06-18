using Zenject;

namespace KrolStudio
{
    public class UpgradeStatsCalculator
    {
        private readonly IProgressService _progressService;

        [Inject]
        public UpgradeStatsCalculator(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public int GetIncomeUpgradeCost() =>
             GetValueForLevel(_progressService.GetIncomePurchaseCount());

        public int GetPartCreationCost() =>
            GetValueForLevel(_progressService.GetPartPurchaseCount());

        private int GetValueForLevel(int level) => 
            (5 * level * level + 5 * level + 10) / 2;
    }
}