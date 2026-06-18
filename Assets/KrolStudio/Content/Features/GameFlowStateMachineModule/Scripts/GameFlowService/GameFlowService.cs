using Cysharp.Threading.Tasks;
using System;
using Zenject;

namespace KrolStudio
{
    public class GameFlowService : IGameFlowService
    {
        private readonly GameFlowStateMachine _gameFlowStateMachine;

        [Inject]
        public GameFlowService(GameFlowStateMachine stateMachine)
        {
            _gameFlowStateMachine = stateMachine;
        }

        public async UniTask EnterGameplay<T>() where T : UIScreen
        {
            await _gameFlowStateMachine.Enter<GameplayFlowState, Type>(typeof(T));
        }
    }
}