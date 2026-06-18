using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KrolStudio
{
    public class Rocket : MonoBehaviour
    {
        [SerializeField] ParticleSystem[] launchParticles;
        [SerializeField] ParticleSystem explosionPrefab;

        private IAudioService _audioService;
        private RocketConfig _config;
        private Transform _parent;
        private SoundEmitter _rocketLaunchEmitter;
        private PlayerStatsCalculator _statsCalculator;
        private UpgradeLevelController _upgradeLevel;

        public void Initialize(
            RocketConfig config, 
            IAudioService audioService,
            PlayerStatsCalculator statsCalculator,
            UpgradeLevelController upgradeLevel)
        {
            _config = config;
            _audioService = audioService;
            _parent = transform.parent;
            _statsCalculator = statsCalculator;
            _upgradeLevel = upgradeLevel;
        }

        public void Launch(Transform target)
        {
            _rocketLaunchEmitter = _audioService.Play(GameConstants.Sounds.RocketLaunch, transform);
            
            transform.SetParent(null);

            Vector3 startPos = transform.localPosition;

            // Point after ascending upwards
            Vector3 upPos = startPos + Vector3.up * _config.upDistance;

            // Intermediate point for the arc
            Vector3 midPoint = (upPos + target.position) / 2f;
            midPoint.y += _config.arcHeight;

            // Full path: up -> arc -> target
            Vector3[] path = new Vector3[]
            {
                upPos,
                midPoint,
                target.position
            };

            transform.DOPath(path, _config.upTime + _config.flyTime, PathType.CatmullRom, PathMode.Full3D)
                 .SetEase(Ease.InOutSine)
                 .SetLookAt(0.01f)
                 .OnStart(() => {
                     LaunchParticles(true);
                     transform.SetParent(null);
                 })
                 .OnComplete(Explode);
        }

        void LaunchParticles(bool value)
        {
            foreach (var item in launchParticles)
            {
                if(value)
                    item.Play(true);
                else
                    item.Stop();
            }
        }

        void Explode()
        {
            LaunchParticles(false);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            _audioService.Stop(_rocketLaunchEmitter);
            _audioService.Play(GameConstants.Sounds.RocketExplosion, transform.position);

            ApplyDamage();

            transform.SetParent(_parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        private void ApplyDamage()
        {
            // Get all colliders inside the sphere
            Collider[] colliders = Physics.OverlapSphere(
                 transform.position,
                 _config.baseRadius,
                 _config.detectableTargets,              // LayerMask
                 QueryTriggerInteraction.Collide        
             );

            foreach (Collider hit in colliders)
            {
                if (hit.gameObject == gameObject) continue; // Skip self (ignore the collider of the current object)

                HandleAttack(hit);
            }
        }

        private void HandleAttack(Collider hit)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
                Attack(damageable);
        }

        private void Attack(IDamageable damageable)
        {
            damageable.Damage(new HitData
            {
                Damage = _statsCalculator.GetRocketDamage(_upgradeLevel.GetLevel(PartType.Rocket)),
                Direction = Vector3.up,
                Force = _config.hitForce
            });
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            // Draw only if this specific object is selected
            if (Selection.activeGameObject != gameObject)
                return;

            if (_config == null)
            {
                Debug.LogWarning($"{name}: Config not set!", this);
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _config.baseRadius);
        }
#endif
    }
}