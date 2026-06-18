using System;
using Zenject;

namespace KrolStudio
{
    public class CompletePresenter : IInitializable, IDisposable
    {
        private readonly CompleteView _view;
        private readonly IGameFlowService _gameFlowService;
        private readonly IUIScreenNavigationService _navigation;
        private readonly SignalBus _signalBus;
        private readonly RewardConfig _rewardConfig;
        private readonly ProgressService _progressService;

        [Inject]
        public CompletePresenter(
               IUIScreenNavigationService navigation,
               IGameFlowService gameFlowService,
               UIManager manager,
               SignalBus signalBus,
               RewardConfig rewardConfig,
               ProgressService progressService)
        {
            _gameFlowService = gameFlowService;
            _navigation = navigation;
            _view = manager.GetScreen<CompleteView>();
            _signalBus = signalBus;
            _rewardConfig = rewardConfig;
            _progressService = progressService;
        }

        public void OnNext()
        {
            _signalBus.Fire(new NextLevelSignal());
            _navigation.PopAndHide();
            _gameFlowService.EnterGameplay<UpgradeScreenView>();
        }

        private void InitReward()
        {
            RewardLevel reward = _rewardConfig.GetReward(_progressService.GetLevel());
            _view.SetRewardMessage(reward.message);
            _view.SetRewardsView(reward.reward);
        }

        public void Initialize()
        {
            _view.NextAction += OnNext;
            _view.InitReward += InitReward;
        }

        public void Dispose()
        {
            _view.NextAction -= OnNext;
            _view.InitReward -= InitReward;
        }
    }
}