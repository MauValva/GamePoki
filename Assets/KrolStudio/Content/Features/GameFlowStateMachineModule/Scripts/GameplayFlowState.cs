using Cysharp.Threading.Tasks;
using Global.Scripts.Generated;
using System;
using System.Collections.Generic;
using Zenject;

namespace KrolStudio
{
    public class GameplayFlowState : IState<Type>
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly SignalBus _signalBus;  
        private readonly LoadingCurtain _curtain;
 

        [Inject]
        public GameplayFlowState(
            ISceneLoaderService sceneLoaderService,
            SignalBus signalBus,
            LoadingCurtain curtain)
        {
            _sceneLoaderService = sceneLoaderService;
            _signalBus = signalBus;
            _curtain = curtain;
        }

        public async UniTask Enter(Type screenType)
        {
            await _curtain.FadeInAsync();

            List<string> enabledScenes = new()
            {
                SceneInBuild.GlobalScene,
                SceneInBuild.UIScene,
                SceneInBuild.GameplayScene,
            };

            await _sceneLoaderService.LoadScenesAsync(enabledScenes, SceneInBuild.GameplayScene, true);

            _signalBus.Fire(new ShowScreenSignal(screenType));

            _curtain.FadeOutAsync().Forget();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
