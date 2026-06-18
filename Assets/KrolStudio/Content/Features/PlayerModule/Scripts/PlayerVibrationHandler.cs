using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PlayerVibrationHandler : MonoBehaviour
    {
        private IDamageable _damageable;
        private IVibrationService _vibration;

        [Inject]
        private void Construct(IVibrationService vibration) =>
            _vibration = vibration;

        private void Awake() =>
            _damageable = GetComponent<IDamageable>();

        private void OnEnable() =>
            _damageable.OnDamaged += OnDamaged;

        private void OnDisable() =>
            _damageable.OnDamaged -= OnDamaged;

        private void OnDamaged(float damage) =>
            _vibration.Vibrate(GameConstants.Vibrations.ShortVibration);
    }
}