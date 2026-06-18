using UnityEngine;

namespace KrolStudio
{

    [RequireComponent(typeof(Rigidbody))]
    public class CenterOfMassSetter : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _centerOfMass;

        [SerializeField]
        private bool _applyOnStart = true;

        [SerializeField]
        private bool _drawGizmo = true;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (_applyOnStart)
            {
                ApplyCenterOfMass();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();

            if (_rb != null && !_rb.isKinematic)
            {
                ApplyCenterOfMass();
            }
        }
#endif

        public void ApplyCenterOfMass()
        {
            if (_rb == null) return;

            _rb.centerOfMass = _centerOfMass;
        }

        public void ResetToDefault()
        {
            if (_rb == null) return;

            _rb.ResetCenterOfMass();
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmo) return;

            Rigidbody rb = _rb != null ? _rb : GetComponent<Rigidbody>();
            if (rb == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(_centerOfMass), 0.05f);
        }
    }
}