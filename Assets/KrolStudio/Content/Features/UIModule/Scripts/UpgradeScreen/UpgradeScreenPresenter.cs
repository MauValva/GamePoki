using Cysharp.Threading.Tasks;
using System;
using Zenject;

namespace KrolStudio
{
    public class UpgradeScreenPresenter : IInitializable, IDisposable
    {
        private readonly UpgradeScreenView _view;
        private readonly IUIScreenNavigationService _navigation;
        private readonly CameraSwitcherProxy _cameraSwitcher;
        private readonly UpgradeStatsCalculator _statsCalculator;
        private readonly IProgressService _progressService;
        private readonly IWalletService _walletService;
        private readonly SignalBus _signalBus;
        private readonly BoardServiceModel _boardService;
        private readonly SlideMoverContext _slideMover;
        private readonly PlayerEffectsModel _playerEffects;

        [Inject]
        public UpgradeScreenPresenter(
            IUIScreenNavigationService navigation,
            UIManager manager,
            CameraSwitcherProxy cameraSwitcher,
            UpgradeStatsCalculator statsCalculator,
            IProgressService progressService,
            IWalletService walletService,
            SignalBus signalBus,
            BoardServiceModel boardService,
            SlideMoverContext slideMover,
            PlayerEffectsModel playerEffects)
        {
            _cameraSwitcher = cameraSwitcher;
            _navigation = navigation;
            _view = manager.GetScreen<UpgradeScreenView>();
            _statsCalculator = statsCalculator;
            _progressService = progressService;
            _walletService = walletService;
            _signalBus = signalBus;
            _boardService = boardService;
            _slideMover = slideMover;
            _playerEffects = playerEffects;
        }

        public void Initialize()
        {
            _view.PlayAction += OnPlay;
            _view.AddIncomeAction += OnAddIncome;
            _view.AddPartAction += OnAddPart;
            _view.PartSelected += OnAddPart;

            _view.SetPartCreationCost(_statsCalculator.GetPartCreationCost());
            _view.SetIncomeUpgradeCost(_statsCalculator.GetIncomeUpgradeCost());

            _walletService.SubscribeToUpdate(CurrencyType.Coin, OnWalletChanged);
            _signalBus.Subscribe<BoardChangedSignal>(OnBoardReady);
            _signalBus.Subscribe<TutorialCompletedSignal>(OnTutorialCompleted);
        }

        public void Dispose()
        {
            _view.PlayAction -= OnPlay;
            _view.AddIncomeAction -= OnAddIncome;
            _view.AddPartAction -= OnAddPart;
            _view.PartSelected -= OnAddPart;

            _walletService.UnsubscribeFromUpdate(CurrencyType.Coin, OnWalletChanged);
            _signalBus.Unsubscribe<BoardChangedSignal>(OnBoardReady);
            _signalBus.Unsubscribe<TutorialCompletedSignal>(OnTutorialCompleted);
        }

        private void OnTutorialCompleted(TutorialCompletedSignal _) =>
            UpdateUpgradeAvailability();

        private void OnWalletChanged(int oldValue, int newValue, object sender) => 
            UpdateUpgradeAvailability();

        private void OnBoardReady(BoardChangedSignal _) => 
            UpdateUpgradeAvailability();

        public void OnPlay()
        {
            _signalBus.Fire(new TutorialStepCompletedSignal { StepId = "Play" });
            PlayAsync().Forget();
        }

        private async UniTask PlayAsync()
        {
            _boardService.Value.HideLevelIndicators();
            
            await _slideMover.StartSlide();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5d), ignoreTimeScale: false);

            _navigation.Replace<TapToStartView>().Forget();
            _cameraSwitcher.SwitchToFollow();
        }

        private void TryAddPart(PartType partType = PartType.None)
        {
            if (!_walletService.TryPurchase(CurrencyType.Coin, _statsCalculator.GetPartCreationCost(), this)) return;

            bool added = partType == PartType.None ? _boardService.Value.TryAddPart() : _boardService.Value.TryAddPart(partType);
            if (!added) return;

            _progressService.SetPartPurchaseCount(_progressService.GetPartPurchaseCount() + 1);
            _view.SetPartCreationCost(_statsCalculator.GetPartCreationCost());
            UpdateUpgradeAvailability();
        }

        public void OnAddPart() => TryAddPart();
        
        public void OnAddPart(PartType partType) => TryAddPart(partType);

        private void UpdateUpgradeAvailability()
        {
            bool hasMoneyForPart = _walletService.HasEnough(CurrencyType.Coin, _statsCalculator.GetPartCreationCost());
            bool hasMoneyForIncome = _walletService.HasEnough(CurrencyType.Coin, _statsCalculator.GetIncomeUpgradeCost());

            _view.SetActiveAddPart(hasMoneyForPart && _boardService.Value.HasFreeSlot);
            _view.SetActiveAddIncome(hasMoneyForIncome);
        }

        public void OnAddIncome()
        {
            if (_walletService.TryPurchase(CurrencyType.Coin, _statsCalculator.GetIncomeUpgradeCost(), this))
            {
                _progressService.SetIncomePurchaseCount(_progressService.GetIncomePurchaseCount() + 1);
                _view.SetIncomeUpgradeCost(_statsCalculator.GetIncomeUpgradeCost());
                _playerEffects.Value.PlayCoinsDirectional();
            }
        }
    }
}