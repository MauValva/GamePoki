using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class DamagePopupHandler : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Color damageColor;

        private IDamageable _damageable;
        private IInteractorPoolContainer _poolContainer;

        [Inject]
        private void Construct(IInteractorPoolContainer poolContainer) =>
            _poolContainer = poolContainer;

        private void Awake() =>
            _damageable = GetComponent<IDamageable>();

        private void OnEnable() =>
            _damageable.OnDamaged += OnDamaged;

        private void OnDisable() =>
            _damageable.OnDamaged -= OnDamaged;

        private void OnDamaged(float damage)
        {
            _poolContainer.GetPool<DamagePopup>()
                .Get(spawnPoint.position)
                .Play(Mathf.FloorToInt(damage), damageColor);
        }
    }
}