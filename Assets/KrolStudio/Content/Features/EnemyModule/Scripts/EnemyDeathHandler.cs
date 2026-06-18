using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        private IDamageable _damageable;
        private IEnemyStateService _stateService;
        private IEnemyRagdoll _ragdoll;
        private IEnemyShadow _shadow;
        private Collider _collider;
        private HitData _lastHit;

        [Inject]
        private void Construct(IEnemyStateService stateService)
        {
            _stateService = stateService;
        }

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
            _ragdoll = GetComponent<EnemyRagdoll>();
            _collider = GetComponent<Collider>();
            _shadow = GetComponent<EnemyShadow>();
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
            _shadow.Hide();
            _collider.enabled = false;
            _ragdoll.EnableRagdoll(true);
            _ragdoll.Push(_lastHit.Direction, _lastHit.Force); 
            _stateService.EnterDead().Forget();
        }
    }
}