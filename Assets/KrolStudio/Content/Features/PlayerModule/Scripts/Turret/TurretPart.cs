using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class TurretPart : MonoBehaviour
    {
        public Transform rotatingPart;
        public Transform trajectoryPoint;
        public bool fireSequentially;

        [Space]
        public FireComponents[] fireComponents;

        private TurretRotator _turretRotator;
        private TurretShooting _turretShooting;
        private ILaserRenderer _laser;

        int firePointIndex;

        [Inject]
        private void Construct(
            TurretRotator turretRotator,
            TurretShooting turretShooting,
            ILaserRenderer laser)
        {
            _turretRotator = turretRotator;
            _turretShooting = turretShooting;
            _laser = laser;
        }

        private void OnEnable()
        {
            _turretRotator.SetRotatingTransform(rotatingPart);

            _laser.SetTrajectoryPoint(trajectoryPoint);

            firePointIndex = 0;

            _turretShooting.SetShotCount(fireSequentially ? 1 : fireComponents.Length);
            _turretShooting.OnFirePoint += GetFirePoint;
        }

        private Transform GetFirePoint()
        {
            firePointIndex = (firePointIndex + 1) % fireComponents.Length;
            fireComponents[firePointIndex].PlayFireFx();
            fireComponents[firePointIndex].PlayFireAnime();
            return fireComponents[firePointIndex].FirePoint;
        }

        private void OnDisable()
        {
            _turretShooting.OnFirePoint -= GetFirePoint;
        }
    }
}
