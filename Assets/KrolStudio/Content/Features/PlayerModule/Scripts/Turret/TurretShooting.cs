using Core.InputModule;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace KrolStudio
{
    public class TurretShooting
    {
        private readonly IInteractorPoolContainer _interactorPool;
        private readonly ILogService _logService;
        private readonly IInputListener _inputListener;
        private readonly AmmoModel _ammoModel;
        private readonly PlayerStatsCalculator _statsCalculator;
        private readonly UpgradeLevelController _upgradeLevel;

        public event Func<Transform> OnFirePoint;
        public event Action OnShot;

        private int _shotCount;
        private bool _isShooting;
        private float _timer;
        private float _fireInterval;
        private float _damage;

        [Inject]
        public TurretShooting(IInteractorPoolContainer interactorPool,
                              ILogService logService,
                              IInputListener inputListener,
                              AmmoModel ammoModel,
                              PlayerStatsCalculator statsCalculator,
                              UpgradeLevelController upgradeLevel)
        {
            _interactorPool = interactorPool;
            _logService = logService;
            _inputListener = inputListener;
            _ammoModel = ammoModel;
            _statsCalculator = statsCalculator;
            _upgradeLevel = upgradeLevel;
        }

        public void SetShotCount(int shotCount) =>
            _shotCount = shotCount;

        public void Enable()
        {
            _damage = _statsCalculator.GetProjectileDamage(_upgradeLevel.GetLevel(PartType.Turret));
            _fireInterval = _statsCalculator.GetFireInterval(_upgradeLevel.GetLevel(PartType.Turret));
            _timer = _fireInterval;
            _isShooting = false;
            _inputListener.OnLeftButtonStarted += OnPointerDown;
            _inputListener.OnLeftButtonCanceled += OnPointerUp;
        }

        private void OnPointerDown(Vector2 position)
        {
            if (EventSystem.current.IsPointerOverGameObject() || 
                EventSystem.current.IsPointerOverGameObject(0))
            {
                // The Player physics layer is specified in the PhysicsRaycaster component attached to the camera.
                // Ignore the Player layer so that the turret can detect hits.
                if (!RaycastUtility.RaycastFromPosition(out var results, _inputListener.CurrentMousePosition, LayerMask.GetMask("Player")))
                    return;

                _logService.LogWarning($"InputDown Fail. {RaycastUtility.GetTopUIObject(_inputListener.CurrentMousePosition)?.name}");
            }

            _isShooting = true;
        }

        private void OnPointerUp(Vector2 position)
        {
            _isShooting = false;
        }

        public void Disable()
        {
            _isShooting = false;
            _inputListener.OnLeftButtonStarted -= OnPointerDown;
            _inputListener.OnLeftButtonCanceled -= OnPointerUp;
        }      

        public void Tick()
        {
            if (_ammoModel.IsEmpty) return;
            _timer += Time.deltaTime;
            if (_isShooting && _timer >= _fireInterval)
                Shot();
        }

        private void Shot()
        {
            _timer = 0f;

            ProjectilePoolHandler projectile;

            for (int i = 0; i < _shotCount; i++)
            {
                projectile = _interactorPool.GetPool<ProjectilePoolHandler>().Get();
                projectile.Initialize(OnFirePoint?.Invoke(), _damage);
                _ammoModel.Spend(1);
                OnShot?.Invoke();
            }
        }
    }
}