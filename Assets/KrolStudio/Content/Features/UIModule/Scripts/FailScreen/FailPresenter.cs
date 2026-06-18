using System;
using Zenject;

namespace KrolStudio
{
    public class FailScreenPresenter : IInitializable, IDisposable
    {
        private readonly FailView _view;
        private readonly IGameFlowService _gameFlowService;
        private readonly IUIScreenNavigationService _navigation;

        [Inject]
        public FailScreenPresenter(
               IUIScreenNavigationService navigation,
               IGameFlowService gameFlowService,
               UIManager manager)
        {
            _gameFlowService = gameFlowService;
            _navigation = navigation;
            _view = manager.GetScreen<FailView>();
        }

        public void OnRestart()
        {
            _navigation.PopAndHide();
            _gameFlowService.EnterGameplay<UpgradeScreenView>();
        }

        public void Initialize()
        {
            _view.RestartAction += OnRestart;
        }

        public void Dispose()
        {
            _view.RestartAction -= OnRestart;
        }
    }
}