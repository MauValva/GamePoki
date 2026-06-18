using UnityEngine;

namespace KrolStudio
{
    public class TentacleRotator
    {
        private readonly PlayerTransformModel _playerTransform;
        private readonly EnemyTransformModel _enemyTransform;
        private readonly TentacleConfig _config;

        public TentacleRotator(
            PlayerTransformModel playerTransform,
            EnemyTransformModel enemyTransform,
            TentacleConfig config)
        {
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _config = config;
        }

        public void LookAtPlayerY()
        {
            Vector3 direction = _playerTransform.Position - _enemyTransform.Position;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.001f) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _enemyTransform.Rotation = Quaternion.Slerp(
                _enemyTransform.Rotation,
                targetRotation,
                Time.deltaTime * _config.rotationSpeed);
        }
    }
}