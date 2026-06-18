using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Turret/" + nameof(TurretConfig),
        fileName = nameof(TurretConfig), order = 0)]
    public class TurretConfig : ScriptableObject
    {
        [Header("Rotation Settings")]
        public float rotationSensitivity = 0.1f;
        public float minRotationAngle = -45f;
        public float maxRotationAngle = 45f;
        public bool inverseRotation = false;
        public float swipeDeadZone = 1f;
        public float rotationSmoothTime = 0.08f;

        [Header("Firing Settings")]
        public float baseFireInterval = 3f;
        public float fireIntervalGrowth = 0.5f;
        [Range(0f, 1f)] public float lowBulletsThreshold = 0.1f;

        [Header("Laser Settings")]
        public float laserMaxDistance = 20f;

        [Header("Bullets")]
        public int baseBulletAmount = 120;
        public float bulletAmountMultiplier = 50f;
    }
}