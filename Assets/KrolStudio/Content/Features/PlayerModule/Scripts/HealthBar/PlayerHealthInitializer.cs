using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [RequireComponent(typeof(PlayerDamageable))]
    public class PlayerHealthInitializer : MonoBehaviour
    {
        private PlayerDamageable _damageable;
        private PlayerDamageableModel _playerDamageableModel;
        private PlayerConfig _config;
        private PlayerStatsCalculator _statsCalculator;
        private UpgradeLevelController _upgradeLevel;

        private void Awake()
        {
            _damageable = GetComponent<PlayerDamageable>();
            _playerDamageableModel.Value = _damageable;
        }

        private void OnEnable()
        {
            InitializeHealth();
        }

        [Inject]
        public void Construct(
            PlayerConfig config, 
            PlayerDamageableModel playerDamageableModel,
            PlayerStatsCalculator statsCalculator,
            UpgradeLevelController upgradeLevel)
        {
            _config = config;
            _playerDamageableModel = playerDamageableModel;
            _statsCalculator = statsCalculator;
            _upgradeLevel = upgradeLevel;
        }

        private void InitializeHealth()
        {
            _damageable.MaxHealth = _statsCalculator.GetHealth(_upgradeLevel.GetLevel(PartType.Armor));
            _damageable.SetHealth(_damageable.MaxHealth);
        }
    }
}