using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public class PlayerFinishState : IState
    {
        private readonly IUIScreenNavigationService _navigation;
        private readonly PlayerChaseTargetModel _playerChaseTarget;
        private readonly PlayerEffectsModel _playerEffects;

#if APPLOVIN || LEVELPLAY || ADMOB
        private readonly LevelCompleteHandler _levelCompleteHandler;
#endif

        public PlayerFinishState(
            IUIScreenNavigationService navigation,
            PlayerChaseTargetModel playerChaseTarget,
            PlayerEffectsModel playerEffects

#if APPLOVIN || LEVELPLAY || ADMOB
            , LevelCompleteHandler levelCompleteHandler
#endif
            )
        {
            _navigation = navigation;
            _playerChaseTarget = playerChaseTarget;
            _playerEffects = playerEffects;

#if APPLOVIN || LEVELPLAY || ADMOB
            _levelCompleteHandler = levelCompleteHandler;
#endif
        }

        public async UniTask Enter()
        {
            _playerChaseTarget.Value.IsOnFinish = true;
            _playerEffects.Value.PlayConfetti();

#if APPLOVIN || LEVELPLAY || ADMOB
            await _levelCompleteHandler.OnLevelComplete();
#endif
            _navigation.Push<CompleteView>().Forget();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}