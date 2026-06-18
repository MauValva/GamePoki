using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PlayerHealthBarPresenter : HealthBarPresenter
    {
        private readonly DisplayablesModel _model;
        private readonly SignalBus _signalBus;

        [Inject]
        public PlayerHealthBarPresenter(
            HealthBarView view, 
            PlayerDamageable damageable, 
            SignalBus signalBus,
            DisplayablesModel model) : base(view, damageable) 
        {
            _model = model;
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            base.Initialize();

            _damageable.OnKilled += OnKilled;

            _signalBus.Subscribe<PlayerHUDVisibilitySignal>(OnVisibilityChanged);
            _model.Register(_view);
        }

        public override void Dispose()
        {
            base.Dispose();

            _damageable.OnKilled -= OnKilled;

            _signalBus.Unsubscribe<PlayerHUDVisibilitySignal>(OnVisibilityChanged);
            _model.Unregister(_view);
        }

        private void OnKilled() =>
           Display(false);

        private void OnVisibilityChanged(PlayerHUDVisibilitySignal signal) => 
            Display(signal.IsVisible);
    }
}