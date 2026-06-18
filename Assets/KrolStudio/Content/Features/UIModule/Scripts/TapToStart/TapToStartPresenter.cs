using System;
using Zenject;

namespace KrolStudio
{
    public class TapToStartPresenter : IInitializable, IDisposable
    {
        private readonly TapToStartView _view;
        private readonly IPlayerStateService _playerStateService;
        private readonly IUIScreenNavigationService _navigation;
        private readonly CameraSwitcherProxy _cameraSwitcher;

        [Inject]
        public TapToStartPresenter(
               IUIScreenNavigationService navigation,
               IPlayerStateService playerStateService,
               UIManager manager,
               CameraSwitcherProxy cameraSwitcher)
        {
            _playerStateService = playerStateService;
            _navigation = navigation;
            _view = manager.GetScreen<TapToStartView>();
            _cameraSwitcher = cameraSwitcher;
        }

        public void OnClose()
        {
            _playerStateService.EnterRun();
            _navigation.Pop();
        }


        public void Initialize()
        {
            _view.CloseAction += OnClose;
        }

        public void Dispose()
        {
            _view.CloseAction -= OnClose;
        }
    }
}