using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class LaserTrajectoryRenderer : MonoBehaviour, ILaserRenderer
    {
        [SerializeField] LineRenderer trajectory;
        [SerializeField] LayerMask hitMask;

        private Transform _trajectoryPoint;
        private bool _isActive;
        private float _maxDistance;

        [Inject]
        private void Construct(TurretConfig config)
        {
            _maxDistance = config.laserMaxDistance;
        }

        public void Tick()
        {
            RenderLaser(_maxDistance);
        }

        public void SetTrajectoryPoint(Transform trajectoryPoint) =>
            _trajectoryPoint = trajectoryPoint;

        public void SetActive(bool active)
        {
            _isActive = active;
            trajectory.enabled = active;
            trajectory.positionCount = active ? 2 : 0;
        }

        public void RenderLaser(float maxDistance)
        {
            if (!_isActive || _trajectoryPoint == null) return;

            Vector3 start = _trajectoryPoint.position;
            Vector3 direction = _trajectoryPoint.forward;
            Vector3 end = start + direction * maxDistance;

            if (Physics.Raycast(start, direction, out RaycastHit hit, maxDistance, hitMask))
                end = hit.point;

            trajectory.SetPosition(0, start);
            trajectory.SetPosition(1, end);
        }
    }
}
