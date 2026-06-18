using System;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;

        private ProjectileConfig _config;
        private ILogService _logService;
        private CameraVisibilityChecker _visibilityChecker;

        private float _flyTime;
        private float _damage;
        private Vector3 _flyDirection;
        private Vector3 _previousPosition;
        private bool _returned;
        private int _layerMask;
        private Collider[] _overlapResults = new Collider[1]; // To avoid allocating every time

        public event Action OnHitCompleted;

        private void Awake()
        {
            _layerMask = LayerMask.GetMask("Enemy");
        }

        [Inject]
        public void Construct(ProjectileConfig config,
                              CameraVisibilityChecker visibilityChecker,
                              ILogService logService)
        {
            _visibilityChecker = visibilityChecker;
            _config = config;
            _logService = logService;
        }

        public void HitCompleted()
        {
            if (_returned) return;
            _returned = true;
            enabled = false;

            OnHitCompleted?.Invoke();
        }

        public void Initialize(Transform firePoint, float damage)
        {
            if (firePoint == null)
            {
                Debug.LogError("FirePoint reference is not set. Please check the TurretPart component.");
                return;
            }

            _damage = damage;
            _flyDirection = firePoint.forward;
            transform.SetPositionAndRotation(firePoint.position, Quaternion.LookRotation(_flyDirection));

            // Reset state
            _returned = false;
            enabled = true;
            _flyTime = 0f;
            _previousPosition = transform.position;
            trailRenderer.Clear();
        }

        void FixedUpdate()
        {
            Move();
            CheckFlyTime();
            TryAttack();
        }

        void Move()
        {
            transform.position += _flyDirection * _config.flySpeed * Time.deltaTime;
        }

        void CheckFlyTime()
        {
            _flyTime += Time.deltaTime;

            if (_flyTime >= _config.flyTime)
            {
                HitCompleted();
            }
        }

        private void TryAttack()
        {
            Vector3 movement = transform.position - _previousPosition;

            if (movement.sqrMagnitude > 0.0001f)
            {
                float distance = movement.magnitude;

                if (Physics.SphereCast(_previousPosition, _config.sphereCastRadius, _flyDirection, 
                    out RaycastHit hit, distance, _layerMask, QueryTriggerInteraction.Collide))
                {
                    HandleAttack(hit.collider);
                }
                else
                {
                    // Fallback if the target is too close and SphereCast did not work
                    int hitsCount = Physics.OverlapSphereNonAlloc(transform.position, _config.sphereCastRadius, 
                        _overlapResults, _layerMask, QueryTriggerInteraction.Collide);
                    if (hitsCount > 0)
                    {
                        HandleAttack(_overlapResults[0]);
                    }
                }
            }

            _previousPosition = transform.position;
        }

        private void HandleAttack(Collider hit)
        {
            if (!hit.TryGetComponent<IDamageable>(out var damageable)) return;
            if (!_visibilityChecker.IsVisible(hit.transform.position)) return;

            damageable.Damage(new HitData
            {
                Damage = _damage,
                Direction = _flyDirection,
                Force = _config.hitForce
            });

            HitCompleted();
        }
    }
}