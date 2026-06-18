using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class CinemachineCameraSetup : MonoBehaviour 
    {
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        private PlayerTransformModel _playerTransformModel;

        [Inject]
        public void InjectDependencies(PlayerTransformModel playerTransformModel) =>
            _playerTransformModel = playerTransformModel;

        private void Start() 
        {
            if (_playerTransformModel.Transform != null)
                SwitchTarget(_playerTransformModel.Transform);
        }

        private void OnEnable() =>
            _playerTransformModel.OnTransformChanged += SwitchTarget;

        private void OnDisable() =>
            _playerTransformModel.OnTransformChanged -= SwitchTarget;

        private void SwitchTarget(Transform target) =>
            _cinemachineCamera.Target.TrackingTarget = target;
    }
}