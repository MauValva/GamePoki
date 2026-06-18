using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class EnemyStatsModel
    {
        public float BaseHealth { get; }
        public float BaseDamage { get; }
        public float HealthGrowth { get; }
        int BaseReward { get; }
        float RewardGrowth { get; }

        [Inject]
        public EnemyStatsModel(IEnemyStatsConfig config)
        {
            BaseHealth = config.BaseHealth;
            BaseDamage = config.BaseDamage;
            HealthGrowth = config.HealthGrowth;
            BaseReward = config.BaseReward;
            RewardGrowth = config.RewardGrowth;
        }

        public int GetEnemyKillReward(bool isDoubleKillReward, int incomeLevel) =>
            Mathf.RoundToInt(BaseReward + RewardGrowth * incomeLevel * (isDoubleKillReward ? 2f : 1f));

        public int GetMaxHealthForLevel(int level) => 
            Mathf.Max(1, (int)(BaseHealth + level * HealthGrowth));

        public int GetDamageByHealthRatio(float healthRatio) => 
            Mathf.RoundToInt(BaseDamage * healthRatio);
    }
}