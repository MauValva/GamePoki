using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class BootstrapSceneBootstrap : MonoBehaviour
    {
        private GameFlowStateMachine _gameFlowStateMachine;
        private IStatesFactory _statesFactory;

        private const int TargetFrameRate = 60;

        [Inject]
        public void Construct(
            GameFlowStateMachine gameFlowStateMachine, 
            IStatesFactory statesFactory)
        {
            _gameFlowStateMachine = gameFlowStateMachine;
            _statesFactory = statesFactory;
        }

        void Awake()
        {
            Application.targetFrameRate = TargetFrameRate;

            _gameFlowStateMachine.RegisterStates(_statesFactory);

            _ = _gameFlowStateMachine.Enter<GlobalGameFlowState>();
        }
    }
}