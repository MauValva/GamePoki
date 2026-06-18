using System;
using Zenject;

namespace KrolStudio
{
    public class HUDPresenter : IInitializable, IDisposable
    {
        private readonly HUDView _view;
        private readonly IWalletService _walletService;
        private readonly IUIScreenNavigationService _navigation;
        private readonly IPauseManager _pauseManager;
        private readonly IProgressService _progressService;

        [Inject]
        public HUDPresenter(
            HUDView view, 
            IWalletService walletService,
            IUIScreenNavigationService navigation,
            IPauseManager pauseManager,
            IProgressService progressService)
        {
            _view = view;
            _walletService = walletService;
            _navigation = navigation;
            _pauseManager = pauseManager;
            _progressService = progressService;
        }

        public void Initialize()
        {
            _walletService.SubscribeToUpdate(CurrencyType.Coin, OnWalletChanged);
            _progressService.OnSetLevel += SetLevel;
            _view.PauseAction += OnPause;

            _view.SetMoney(_walletService.GetAmount(CurrencyType.Coin));
            SetLevel(_progressService.GetLevel());
        }

        private void SetLevel(int level) =>
            _view.SetLevel(level + 1);

        private void OnPause()
        {
            _pauseManager.Pause();
            _navigation.PushPopup<SettingsView>();
        }

        public void Dispose()
        {
            _walletService.UnsubscribeFromUpdate(CurrencyType.Coin, OnWalletChanged);
            _progressService.OnSetLevel -= SetLevel;
            _view.PauseAction -= OnPause;
        }

        private void OnWalletChanged(int oldValue, int newValue, object sender) =>
            _view.SetMoney(newValue);
    }
}