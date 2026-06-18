using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [RequireComponent(typeof(EnemyDamageable))]
    public class EnemyHealthInitializer : MonoBehaviour
    {
        private EnemyDamageable _damageable;
        private EnemyStatsCalculator _enemyStats;

        private void Awake()
        {
            _damageable = GetComponent<EnemyDamageable>();
        }

        private void OnEnable()
        {
            InitializeHealth();
        }

        [Inject]
        public void Construct(EnemyStatsCalculator enemyStats)
        {
            _enemyStats = enemyStats;
        }

        private void InitializeHealth()
        {
            int health = _enemyStats.GetMaxHealth();
            _damageable.MaxHealth = health;
            _damageable.SetHealth(health);
        }
    }
}