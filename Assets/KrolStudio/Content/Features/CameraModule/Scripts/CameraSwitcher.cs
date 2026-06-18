using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class CameraSwitcher : MonoBehaviour, ICameraSwitcher
    {
        [SerializeField] private CinemachineBrain cinemachineBrain;

        [SerializeField] private float followBlendTime = 0.4f;
        [SerializeField] private CinemachineBlendDefinition.Styles followBlendStyle;

        [SerializeField] private float finishBlendTime = 1f;
        [SerializeField] private CinemachineBlendDefinition.Styles finishBlendStyle;

        [Space]
        [SerializeField] private CinemachineCamera startCamera;
        [SerializeField] private CinemachineCamera followCamera;
        [SerializeField] private CinemachineCamera finishCamera;

        [Inject]
        private void Construct(CameraSwitcherProxy proxy) =>
            proxy.SetReal(this);

        public void SwitchToStart()
        {
            startCamera.Priority = 10;
            followCamera.Priority = 0;
            finishCamera.Priority = 0;
            cinemachineBrain.DefaultBlend = new(CinemachineBlendDefinition.Styles.EaseIn, followBlendTime);
        }

        public void SwitchToFollow()
        {
            startCamera.Priority = 0;
            followCamera.Priority = 10;
            finishCamera.Priority = 0;
        }

        public void SwitchToFinish()
        {
            startCamera.Priority = 0;
            followCamera.Priority = 0;
            finishCamera.Priority = 10;
            cinemachineBrain.DefaultBlend = new(CinemachineBlendDefinition.Styles.EaseIn, finishBlendTime);
        }
    }
}