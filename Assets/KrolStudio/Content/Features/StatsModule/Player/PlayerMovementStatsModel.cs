
namespace KrolStudio
{
    public class PlayerMovementStatsModel
    {
        private readonly PlayerConfig _config;

        public PlayerMovementStatsModel(PlayerConfig config) =>
            _config = config;

        public float GetSpeed(int level) =>
            level < 0 ? 0f :
            _config.baseMoveSpeed + (level + 1) * _config.speedGrowth;
    }
}