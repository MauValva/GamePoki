using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class GlobalSceneBootstrap : MonoBehaviour
    {
        private IGameFlowService _gameFlowService;

        [Inject]
        public void Construct(IGameFlowService gameFlowService)
        {
            _gameFlowService = gameFlowService;
        }

        void Start()
        {
            _gameFlowService.EnterGameplay<UpgradeScreenView>();
        }
    }
}