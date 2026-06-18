
namespace KrolStudio
{
    public class EnemyStatsCalculator
    {
        private readonly LevelModel _levelModel;
        private readonly EnemyStatsModel _statsModel;

        public EnemyStatsCalculator(
            LevelModel levelModel, 
            EnemyStatsModel statsModel)
        {
            _levelModel = levelModel;
            _statsModel = statsModel;
        }

        public int GetEnemyKillReward(bool isDoubleKillReward) =>
           _statsModel.GetEnemyKillReward(isDoubleKillReward, 1);

        public int GetMaxHealth() => 
            _statsModel.GetMaxHealthForLevel(_levelModel.CurrentIndex);

        public int GetCurrentDamage(float healthRatio) => 
            _statsModel.GetDamageByHealthRatio(healthRatio);
    }
}