using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class EnemyHitHandler : MonoBehaviour
    {
        private IDamageable _damageable;
        private IEnemyStateService _stateService;

        [Inject]
        private void Construct(IEnemyStateService stateService)
        {
            _stateService = stateService;
        }

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
        }

        private void OnEnable()
        {
            _damageable.OnDamaged += OnHit;
        }

        private void OnDisable()
        {
            _damageable.OnDamaged -= OnHit;
        }

        private void OnHit(float damage)
        {
            _stateService.EnterHit().Forget();
        }
    }
}