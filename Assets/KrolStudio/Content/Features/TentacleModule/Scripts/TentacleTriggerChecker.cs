using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class TentacleTriggerChecker
    {
        private readonly PlayerDamageableModel _playerDamageable;
        private readonly PlayerTransformModel _playerTransformModel;
        private readonly EnemyTransformModel _enemyTransformModel;
        private readonly TentacleConfig _config;

        [Inject]
        public TentacleTriggerChecker(
            PlayerDamageableModel playerDamageable,
            PlayerTransformModel playerTransformModel,
            EnemyTransformModel enemyTransformModel,
            TentacleConfig config)
        {
            _config = config;
            _playerDamageable = playerDamageable;
            _enemyTransformModel = enemyTransformModel;
            _playerTransformModel = playerTransformModel;
        }

        public bool CanTriggered()
        {
            //GizmosUtility.DrawCircle(_enemyTransformModel.Position, _config.triggerDistance, _enemyTransformModel.Transform.up, Color.red);
            return _playerDamageable.Value.IsActive && Vector3.Distance(_enemyTransformModel.Position, _playerTransformModel.Position) <= _config.triggerDistance;
        }

        private void DrawCircle(Vector3 center, float radius, Vector3 up, Color color, int segments = 24)
        {
            float angleStep = 360f / segments;
            Quaternion rotation = Quaternion.AngleAxis(angleStep, up);
            Vector3 forward = Vector3.forward * radius;

            Vector3 prevPoint = center + forward;

            for (int i = 0; i <= segments; i++)
            {
                forward = rotation * forward;
                Vector3 nextPoint = center + forward;
                Debug.DrawLine(prevPoint, nextPoint, color);
                prevPoint = nextPoint;
            }
        }
    }
}
