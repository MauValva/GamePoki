using UnityEngine;

namespace KrolStudio
{
    public class ProjectileStatsModel
    {
        private readonly ProjectileConfig _config;

        public ProjectileStatsModel(ProjectileConfig config) =>
            _config = config;

        public int GetDamage(int level) =>
            level < 0 ? 0 :
            Mathf.RoundToInt(_config.baseDamage + level * _config.damageGrowth);
    }
}