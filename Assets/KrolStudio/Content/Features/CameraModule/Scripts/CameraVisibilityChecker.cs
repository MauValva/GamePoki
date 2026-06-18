using UnityEngine;

namespace KrolStudio
{
    public class CameraVisibilityChecker
    {
        private readonly PlayerCameraModel _cameraModel;

        public CameraVisibilityChecker(PlayerCameraModel cameraModel) =>
            _cameraModel = cameraModel;

        public bool IsVisible(Vector3 position)
        {
            Vector3 viewportPos = _cameraModel.CurrentCamera.WorldToViewportPoint(position);
            return viewportPos.z > 0f
                && viewportPos.x is >= 0f and <= 1f
                && viewportPos.y is >= 0f and <= 1f;
        }
    }
}