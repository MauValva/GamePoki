using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PlayerChaseTarget : MonoBehaviour
    {
        private PlayerDamageable _damageable;
        [SerializeField] private Collider _playerCollider; 

        public bool IsOnFinish { get; set; }

        private PlayerChaseTargetModel _model;

        [Inject]
        private void Construct(PlayerChaseTargetModel model)
        {
            _model = model;
        }

        private void Awake()
        {
            _damageable = GetComponent<PlayerDamageable>();
            _model.Value = this;
        }

        public bool IsPossibleToChase() =>
            _damageable.CurrentHealth > 0 && !IsOnFinish;

        public Vector3 GetClosestTargetPoint(Vector3 near) =>
            _playerCollider.ClosestPoint(near);
    }
}
