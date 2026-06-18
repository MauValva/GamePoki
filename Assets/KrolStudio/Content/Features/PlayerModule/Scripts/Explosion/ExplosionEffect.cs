using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class ExplosionEffect : MonoBehaviour
    {
        [SerializeField] public Transform origin;   // Point relative to which the hit area for fragments after the explosion is calculated
        [SerializeField] List<ExplosionPartInfo> explosionInfo;

        List<Vector3> targets = new List<Vector3>();

        private FragmentExplosionConfig _config;
        private ExplosionFragmentLauncher _fragmentLauncher;
        private PlayerDamageable _damageable;

        private void Awake()
        {
            _fragmentLauncher = new ExplosionFragmentLauncher(_config.angleInDegrees);
            _damageable.OnKilled += OnKilled;
        }

        private void OnDisable()
        {
            _damageable.OnKilled -= OnKilled;
        }

        [Inject]
        private void Construct(
            FragmentExplosionConfig config, 
            PlayerDamageable damageable)
        {
            _config = config;
            _damageable = damageable;
        }

        private void OnKilled()
        {
            Explode().Forget();
        }

        public async UniTask Explode()
        {
            // So that DamageFlashEffect.OnKilled has time to execute.
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f), ignoreTimeScale: false);

            foreach (var parts in explosionInfo)
            {
                var renderers = parts.explodeRenderer;
                if (renderers != null)
                    foreach (var r in renderers)
                        if (r != null && r.gameObject.activeSelf) r.material = _config.grayMaterial;

                var rigidbodies = parts.explodeParts;
                if (rigidbodies == null) continue;

                foreach (var rb in rigidbodies)
                {
                    if (rb == null || !rb.gameObject.activeSelf)
                    {
                        Debug.LogWarning("Rigidbody is null.", this);
                        continue;
                    }

                    rb.isKinematic = false;
                    var target = GetRandomDirectionOffset(origin);
                    _fragmentLauncher.Launch(rb, rb.position, rb.position + target, _config.rotationCount);
                }
            }
        }

        Vector3 GetRandomDirectionOffset(Transform transform)
        {
            float angle = Random.Range(_config.minSpawnAngle, _config.maxSpawnAngle);

            // Uniform distribution across the area:
            float radius = Mathf.Sqrt(Random.Range(0f, 1f)) * (_config.secondRadius - _config.firstRadius) + _config.firstRadius;

            Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
            return rotation * (transform.forward * radius);
        }
    }
}