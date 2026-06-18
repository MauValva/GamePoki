using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class WheelRotator : MonoBehaviour, IWheelRotator
    {
        [SerializeField] private Transform[] wheels;

        private float _wheelRadius;
        private float _speed;
        private float _circumference;

        [Inject]
        private void Construct(PlayerConfig config)
        {
            _speed = config.baseMoveSpeed;
        }

        private void Awake()
        {
            var renderer = wheels[0].gameObject.GetComponent<MeshRenderer>();
            Vector3 extents = renderer.bounds.extents;
            _wheelRadius = Mathf.Max(extents.x, extents.y, extents.z);

            _circumference = 2f * Mathf.PI * _wheelRadius;
        }

        public void Tick()
        {
            RotateWheels();
        }

        private void RotateWheels()
        {
            // distance the car traveled during the frame
            float distance = _speed * Time.deltaTime;

            // rotation angle
            float rotation = (distance / _circumference) * 360f;

            foreach (var wheel in wheels)
            {
                wheel.Rotate(Vector3.right, rotation, Space.Self);
            }
        }

        public void SetSpeed(float newSpeed)
        {
            _speed = newSpeed;
        }
    }
}