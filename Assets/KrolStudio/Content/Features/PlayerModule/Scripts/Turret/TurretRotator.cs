using Core.InputModule;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class TurretRotator
    {
        private readonly TurretConfig _config;
        private readonly IInputListener _inputListener;

        private Transform _rotatingTransform;

        private float _currentRotationY;
        private float _targetRotationY;
        private float _rotationVelocity;

        private Vector2 _previousPosition;

        private bool _isPointerDown;

        [Inject]
        public TurretRotator(
            TurretConfig config,
            IInputListener inputListener)
        {
            _config = config;
            _inputListener = inputListener;
        }

        public void SetRotatingTransform(Transform rotatingTransform)
        {
            _rotatingTransform = rotatingTransform;
            _currentRotationY = _rotatingTransform.localEulerAngles.y;
            _targetRotationY = _currentRotationY;
        }

        public void Enable()
        {
            _inputListener.OnLeftButtonStarted += OnPointerDown;
            _inputListener.OnMousePositionPerformed += OnPointerMove;
            _inputListener.OnLeftButtonCanceled += OnPointerUp;
        }

        public void Disable()
        {
            _inputListener.OnLeftButtonStarted -= OnPointerDown;
            _inputListener.OnMousePositionPerformed -= OnPointerMove;
            _inputListener.OnLeftButtonCanceled -= OnPointerUp;
        }

        private void OnPointerDown(Vector2 position)
        {
            _previousPosition = position;
            _isPointerDown = true;
        }

        private void OnPointerUp(Vector2 position)
        {
            _isPointerDown = false;
        }

        private void OnPointerMove(Vector2 position)
        {
            if (!_isPointerDown) return;

            float deltaX = position.x - _previousPosition.x;

            if (Mathf.Abs(deltaX) > _config.swipeDeadZone)
            {
                RotateByInput(deltaX);
            }

            _previousPosition = position;
        }

        private void RotateByInput(float deltaX)
        {
            float direction = _config.inverseRotation ? 1 : -1;

            float deltaRotation = direction * deltaX * _config.rotationSensitivity;

            _targetRotationY -= deltaRotation;

            _targetRotationY = Mathf.Clamp(
                _targetRotationY,
                _config.minRotationAngle,
                _config.maxRotationAngle
            );
        }

        public void Tick()
        {
            if (_rotatingTransform == null) return;

            _currentRotationY = Mathf.SmoothDamp(
                _currentRotationY,
                _targetRotationY,
                ref _rotationVelocity,
                _config.rotationSmoothTime
            );

            _rotatingTransform.localRotation = Quaternion.Euler(0f, _currentRotationY, 0f);
        }
    }
}