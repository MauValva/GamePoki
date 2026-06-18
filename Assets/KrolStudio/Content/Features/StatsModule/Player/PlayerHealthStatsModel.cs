using UnityEngine;

namespace KrolStudio
{
    public class PlayerHealthStatsModel
    {
        private readonly PlayerConfig _config;

        public PlayerHealthStatsModel(PlayerConfig config) =>
            _config = config;

        public int GetHealth(int level) =>
            Mathf.RoundToInt(_config.baseHealth + (level + 1) * _config.healthGrowth);
    }
}