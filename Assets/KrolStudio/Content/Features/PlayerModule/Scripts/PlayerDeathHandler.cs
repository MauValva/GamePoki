using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        private IDamageable _damageable;
        private IPlayerStateService _stateService;

        [Inject]
        private void Construct(IPlayerStateService stateService)
        {
            _stateService = stateService;
        }

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
        }

        private void OnEnable()
        {
            _damageable.OnKilled += OnKilled;
        }

        private void OnDisable()
        {
            _damageable.OnKilled -= OnKilled;
        }

        private void OnKilled()
        {
            _stateService.EnterDead().Forget();
        }
    }
}