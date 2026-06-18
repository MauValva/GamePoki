using System;
using Zenject;

namespace KrolStudio
{
    public class HealthBarPresenter : IInitializable, IDisposable
    {
        protected readonly HealthBarView _view;
        protected readonly IDamageable _damageable;

        [Inject]
        public HealthBarPresenter(
            HealthBarView view, 
            IDamageable damageable)
        {
            _view = view;
            _damageable = damageable;
        }

        public virtual void Initialize()
        {
            _damageable.OnDamaged += OnDamaged;
            _damageable.OnHealthChanged += OnHealthChanged;

            Display(false);
        }

        public void Display(bool value) => 
            _view.Display(value);
  
        private void OnDamaged(float damage)
        {
            _view.UpdateHealth(_damageable.CurrentHealth);
        }

        private void OnHealthChanged(float current, float max)
        {
            _view.SetMaxHealth(max);
            _view.UpdateHealth(current);
        }

        public virtual void Dispose()
        {
            _damageable.OnDamaged -= OnDamaged;
            _damageable.OnHealthChanged -= OnHealthChanged;
        }
    }
}