using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/CullingConfig",
        fileName = nameof(CullingConfig))]
    public class CullingConfig : ScriptableObject
    {
        [Header("Check")]
        [Tooltip("Checks every N ticks. At 60fps and a value of 6, this equals 10 checks per second")]
        public int CheckInterval = 6;

        [Header("Enemy Culling")]
        public float EnemyCullDistance = 40f;
        [Tooltip("Hysteresis for restore (to prevent flickering)")]
        public float EnemyRestoreOffset = 5f;

        [Header("Environment Culling")]
        [Tooltip("Never cull closer than this distance.\r\n")]
        public float EnvMinDistance = 15f;
        [Tooltip("Always cull beyond this distance.\r\n")]
        public float EnvMaxDistance = 80f;

        public float EnemyCullDistanceSq => EnemyCullDistance * EnemyCullDistance;
        public float EnvMinDistanceSq => EnvMinDistance * EnvMinDistance;
        public float EnvMaxDistanceSq => EnvMaxDistance * EnvMaxDistance;
    }
}