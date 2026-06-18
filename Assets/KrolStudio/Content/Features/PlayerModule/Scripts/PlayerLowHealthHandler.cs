using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PlayerLowHealthHandler : MonoBehaviour
    {
        private IDamageable _damageable;
        private IPlayerEffects _effects;
        private PlayerConfig _config;

        [Inject]
        private void Construct(IPlayerEffects effects, PlayerConfig config)
        {
            _effects = effects;
            _config = config;
        }

        private void Awake() =>
            _damageable = GetComponent<IDamageable>();

        private void OnEnable()
        {
            _damageable.OnHealthChanged += OnHealthChanged;
            _effects.PlaySpikyFireBig(false); // Reset on spawn.
        }

        private void OnDisable()
        {
            _damageable.OnHealthChanged -= OnHealthChanged;
            _effects.PlaySpikyFireBig(false);
        }

        private void OnHealthChanged(float current, float max)
        {
            if (max <= 0f) return;

            bool isLowHealth = current / max <= _config.lowHealthThreshold;
            _effects.PlaySpikyFireBig(isLowHealth);
        }
    }
}