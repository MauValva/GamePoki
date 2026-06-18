using UnityEngine;

namespace KrolStudio
{
    public class EnemyMover
    {
        private readonly EnemyTransformModel _transform;

        public EnemyMover(EnemyTransformModel transform)
        {
            _transform = transform;
        }

        public bool HasReachedPoint(Vector3 destination, float threshold) =>
            Vector3.Distance(_transform.Position, destination) < threshold;

        public void MoveTowards(Vector3 targetPosition, float speed, float rotationSpeed)
        {
            Vector3 direction = (targetPosition - _transform.Position).normalized;
            if (direction == Vector3.zero) return;

            _transform.Rotation = Quaternion.Slerp(
                _transform.Rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime
            );

            _transform.Position += _transform.Transform.forward * speed * Time.deltaTime;
        }
    }
}