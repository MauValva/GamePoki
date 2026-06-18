using UnityEngine;

namespace KrolStudio
{
    public class RocketStatsModel
    {
        private readonly RocketConfig _config;

        public RocketStatsModel(RocketConfig config) =>
            _config = config;

        public int GetDamage(int level) =>
            level < 0 ? 0 :
            Mathf.RoundToInt(_config.baseDamage + level * _config.damageGrowthPerLevel);
    }
}