using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class PlayerRunState : IState, IUpdatableState
    {
        private readonly IMovementService _movement;
        private readonly IPlayerStateService _playerStateService;
        private readonly TurretRotator _turretRotator;
        private readonly TurretShooting _turretShooting;
        private readonly IPlayerEffects _playerEffects;
        private readonly ILaserRenderer _laser;
        private readonly IWheelRotator _wheelRotator;
        private readonly IRocketLaunchService _rocketLaunchService;
        private readonly PlayerStatsCalculator _statsCalculator;
        private readonly UpgradeLevelController _upgradeLevel;

        [Inject]
        public PlayerRunState(
            IMovementService movement,
            IPlayerStateService playerStateService,
            TurretRotator turretRotator,
            TurretShooting turretShooting,
            IPlayerEffects playerEffects,
            ILaserRenderer laser,
            IWheelRotator wheelRotator,
            PlayerConfig playerConfig,
            IRocketLaunchService rocketLaunchService,
            PlayerStatsCalculator statsCalculator,
            UpgradeLevelController upgradeLevel)
        {
            _movement = movement;
            _playerStateService = playerStateService;
            _laser = laser;
            _turretRotator = turretRotator;
            _turretShooting = turretShooting;
            _playerEffects = playerEffects;
            _wheelRotator = wheelRotator;
            _rocketLaunchService = rocketLaunchService;
            _statsCalculator = statsCalculator;
            _upgradeLevel = upgradeLevel;
        }

        public UniTask Enter()
        {
            _turretRotator.Enable();
            _turretShooting.Enable();
            _laser.SetActive(true);

            _movement.SetSpeed(_statsCalculator.GetSpeed(_upgradeLevel.GetLevel(PartType.Wheel)));
            _movement.SplineMoveCompleted += OnCompleted;

            _playerEffects.PlayWheelTrail(true);
            _playerEffects.PlaySmokeDarkTrail(true);

            return default;
        }

        private void OnCompleted()
        {
            _movement.SplineMoveCompleted -= OnCompleted;
            _playerStateService.EnterFinish();
        }

        public void Update()
        {
            _movement.Tick();
            _turretRotator.Tick();
            _laser.Tick();
            _turretShooting.Tick();
            _wheelRotator.Tick();
            _rocketLaunchService.Tick();
        }

        public UniTask Exit()
        {
            _turretRotator.Disable();
            _turretShooting.Disable();

            _laser.SetActive(false);

            _playerEffects.PlaySmokeDarkTrail(false);

            return default;
        }
    }
}