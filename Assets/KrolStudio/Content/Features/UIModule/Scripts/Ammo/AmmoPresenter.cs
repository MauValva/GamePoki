using System;
using Zenject;

namespace KrolStudio
{
    public class AmmoPresenter : IInitializable, IDisposable
    {
        private readonly AmmoView _view;
        private readonly DisplayablesModel _displayables;
        private readonly SignalBus _signalBus;
        private readonly AmmoModel _ammoModel;
        private readonly PlayerStatsCalculator _stats;
        private readonly PlayerDamageable _damageable;
        private readonly UpgradeLevelController _upgradeLevel;

        [Inject]
        public AmmoPresenter(
            AmmoView view,
            AmmoModel ammoModel,
            PlayerStatsCalculator stats,
            DisplayablesModel displayables,
            SignalBus signalBus,
            PlayerDamageable damageable,
            UpgradeLevelController upgradeLevel)
        {
            _view = view;
            _ammoModel = ammoModel;
            _stats = stats;
            _displayables = displayables;
            _signalBus = signalBus;
            _damageable = damageable;
            _upgradeLevel = upgradeLevel;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerHUDVisibilitySignal>(OnVisibilityChanged);
            _displayables.Register(_view);

            _damageable.OnKilled += OnKilled;

            // We initialize the model with current values.
            int total = _stats.GetAmmo(_upgradeLevel.GetLevel(PartType.Ammo));
            int lowThreshold = _stats.GetLowBulletsThreshold(total);
            _ammoModel.Initialize(total, lowThreshold);

            // We subscribe the View to the model.
            _ammoModel.OnAmmoChanged += _view.UpdateAmmoUI;
            _ammoModel.OnAmmoEmpty += _view.ShowOutOfBulletsMessage;
        }

        public void Dispose()
        {
            _damageable.OnKilled -= OnKilled;

            _signalBus.Unsubscribe<PlayerHUDVisibilitySignal>(OnVisibilityChanged);
            _displayables.Unregister(_view);

            _ammoModel.OnAmmoChanged -= _view.UpdateAmmoUI;
            _ammoModel.OnAmmoEmpty -= _view.ShowOutOfBulletsMessage;
        }

        private void OnKilled() =>
            _view.Display(false);

        private void OnVisibilityChanged(PlayerHUDVisibilitySignal signal) =>
           _view.Display(signal.IsVisible);
    }
}
