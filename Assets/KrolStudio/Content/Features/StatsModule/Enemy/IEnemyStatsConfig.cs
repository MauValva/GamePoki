
namespace KrolStudio
{
    public interface IEnemyStatsConfig
    {
        float BaseHealth { get; }
        float BaseDamage { get; }
        float HealthGrowth { get; }
        int BaseReward { get; }
        float RewardGrowth { get; }
    }
}