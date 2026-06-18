using UnityEngine;

namespace KrolStudio
{
    public class TurretStatsModel
    {
        private readonly TurretConfig _config;

        public TurretStatsModel(TurretConfig config) =>
            _config = config;

        public int GetAmmo(int level) =>
            level < 0 ? 0 :
            Mathf.RoundToInt(_config.baseBulletAmount + level * _config.bulletAmountMultiplier);

        public float GetFireInterval(int level) =>
            level < 0 ? 0f :
            Mathf.Max(0.05f, _config.baseFireInterval - level * _config.fireIntervalGrowth);

        public int GetLowBulletsThreshold(int totalBullets) =>
            Mathf.RoundToInt(_config.lowBulletsThreshold * totalBullets);
    }
}