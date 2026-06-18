using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [RequireComponent(typeof(RectTransform))]
    public class RectBillboard : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private PlayerCameraModel _playerCameraModel;

        [Inject]
        void Construct(PlayerCameraModel playerCameraModel)
        {
            _playerCameraModel = playerCameraModel;
        }

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        void LateUpdate()  // RotateTowardsCamera
        {
            // Rotate the object to face the camera direction
            Vector3 dir = _rectTransform.position - _playerCameraModel.CurrentCamera.transform.position;
            _rectTransform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}