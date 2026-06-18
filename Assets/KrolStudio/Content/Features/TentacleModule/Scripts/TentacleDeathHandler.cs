using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class TentacleDeathHandler : MonoBehaviour
    {
        private IDamageable _damageable;
        private ITentacleStateService _stateService;
        private Collider _collider;
        private HitData _lastHit;

        [Inject]
        private void Construct(ITentacleStateService stateService)
        {
            _stateService = stateService;
        }

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
            _collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            _collider.enabled = true;
            _damageable.OnKilled += OnKilled;
            _damageable.OnHitReceived += OnHitReceived;
        }

        private void OnDisable()
        {
            _damageable.OnKilled -= OnKilled;
            _damageable.OnHitReceived -= OnHitReceived;
        }

        private void OnHitReceived(HitData hitData) =>
            _lastHit = hitData;

        private void OnKilled()
        {
            _collider.enabled = false;
            _stateService.EnterDead().Forget();
        }
    }
}