using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PlayerCameraRegister : MonoBehaviour 
    {
        [SerializeField] private Camera _camera;
        private PlayerCameraModel _playerCameraModel;

        [Inject]
        public void Construct(PlayerCameraModel playerCameraModel) =>
            _playerCameraModel = playerCameraModel;

        private void Awake() =>
            _playerCameraModel.CurrentCamera = _camera;
    }
}