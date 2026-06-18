using System;
using UnityEngine;

namespace KrolStudio
{
    [RequireComponent(typeof(Projectile))]
    public class ProjectilePoolHandler : MonoBehaviour, IPoolReturnable<ProjectilePoolHandler>
    {
        public event Action<ProjectilePoolHandler> OnReturned;

        private Projectile _projectile;

        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
            _projectile.OnHitCompleted += ReturnToPool;
        }

        private void OnDestroy()
        {
            _projectile.OnHitCompleted -= ReturnToPool;
        }

        public void Initialize(Transform firePoint, float damage) =>
            _projectile.Initialize(firePoint, damage);

        public void ReturnToPool() =>
           OnReturned?.Invoke(this);
    }
}