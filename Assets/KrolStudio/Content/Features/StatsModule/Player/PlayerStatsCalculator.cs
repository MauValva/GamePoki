
namespace KrolStudio
{
    public class PlayerStatsCalculator
    {
        private readonly PlayerHealthStatsModel _health;
        private readonly PlayerMovementStatsModel _movement;
        private readonly TurretStatsModel _turret;
        private readonly ProjectileStatsModel _projectile;
        private readonly RocketStatsModel _rocket;

        public PlayerStatsCalculator(
            PlayerHealthStatsModel health,
            PlayerMovementStatsModel movement,
            TurretStatsModel turret,
            ProjectileStatsModel projectile,
            RocketStatsModel rocket)
        {
            _health = health;
            _movement = movement;
            _turret = turret;
            _projectile = projectile;
            _rocket = rocket;
        }

        public int GetHealth(int partLevel) => _health.GetHealth(partLevel);
        public float GetSpeed(int partLevel) => _movement.GetSpeed(partLevel);
        public int GetAmmo(int partLevel) => _turret.GetAmmo(partLevel);
        public float GetFireInterval(int partLevel) => _turret.GetFireInterval(partLevel);
        public int GetLowBulletsThreshold(int total) => _turret.GetLowBulletsThreshold(total);
        public int GetProjectileDamage(int partLevel) => _projectile.GetDamage(partLevel);
        public int GetRocketDamage(int partLevel) => _rocket.GetDamage(partLevel);
    }
}