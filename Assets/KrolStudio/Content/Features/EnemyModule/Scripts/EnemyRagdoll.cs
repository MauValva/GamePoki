using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class EnemyRagdoll : MonoBehaviour, IEnemyRagdoll
    {
        [SerializeField] private Rigidbody _ragdollHolder;
        [SerializeField] private EnemyAnimator _animator;

        private ILogService _logService;

        public enum Side { Left, Right, Ahead }

        [Inject]
        private void Construct(ILogService logService) =>
            _logService = logService;

        private void OnEnable() =>
            EnableRagdoll(false); // when spawning from the pool - turn it off

        public void EnableRagdoll(bool value)
        {
            if (_ragdollHolder == null)
            {
                _logService.LogWarning("RagdollHolder is null.");
                return;
            }

            _ragdollHolder.gameObject.SetActive(value);
            _animator.Enable(!value);
        }

        public void Push(Vector3 direction, float forceMagnitude)
        {
            _ragdollHolder.linearVelocity = direction * forceMagnitude;
        }

        public void Push(Transform playerTransform, float forceMagnitude)
        {
            Vector3 direction = GetPushDirection(playerTransform);
            Push(direction, forceMagnitude); // We delegate to the first method.
        }

        private Vector3 GetPushDirection(Transform playerTransform)
        {
            Side side = GetTargetSide(playerTransform, transform.position);

            return side switch
            {
                Side.Left => playerTransform.right,
                Side.Right => -playerTransform.right,
                _ => Random.value < 0.5f ? playerTransform.right : -playerTransform.right
            };
        }

        private Side GetTargetSide(Transform observer, Vector3 targetPosition)
        {
            Vector3 forward = observer.forward;
            forward.y = 0;

            Vector3 toTarget = targetPosition - observer.position;
            toTarget.y = 0;

            Vector3 left = Quaternion.AngleAxis(90, Vector3.up) * forward;

            float dot = Vector3.Dot(left.normalized, toTarget.normalized);

            if (dot > 0.01f)
                return Side.Left;
            else if (dot < -0.01f)
                return Side.Right;
            else
                return Side.Ahead;
        }
    }
}