using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class OrbitTowardCamera : MonoBehaviour
    {
        public Transform relativeTo;

        private Transform _targetCamera;
        private float _orbitRadius;
        private PlayerCameraModel _playerCamera;

        [Inject]
        private void Construct(PlayerCameraModel playerCamera)
        {
            _playerCamera = playerCamera;
        }

        private void Awake()
        {
            if (_targetCamera == null)
                _targetCamera = _playerCamera.CurrentCamera.transform;

            if (relativeTo != null)
                _orbitRadius = (transform.position - relativeTo.position).magnitude;
        }

        private void LateUpdate()
        {
            Vector3 from = (transform.position - relativeTo.position).normalized;
            Vector3 to = (_targetCamera.position - relativeTo.position).normalized;

            Quaternion rotation = Quaternion.FromToRotation(from, to);
            transform.position = relativeTo.position + rotation * from * _orbitRadius;

            transform.LookAt(_targetCamera.position, Vector3.up);
            transform.Rotate(0f, 180f, 0f);
        }
    }
}