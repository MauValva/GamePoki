using Cysharp.Threading.Tasks;
using Global.Scripts.Generated;

namespace KrolStudio
{
    public class GlobalGameFlowState : IState
    {
        private readonly ISceneLoaderService _sceneLoaderService;

        public GlobalGameFlowState(ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }
        
        public async UniTask Enter()
        {
            await _sceneLoaderService.LoadSceneAsync(SceneInBuild.GlobalScene, true);
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}