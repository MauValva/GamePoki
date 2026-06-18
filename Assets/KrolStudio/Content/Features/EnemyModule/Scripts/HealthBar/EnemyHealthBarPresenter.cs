using Zenject;

namespace KrolStudio
{
    public class EnemyHealthBarPresenter : HealthBarPresenter
    {
        [Inject]
        public EnemyHealthBarPresenter(
            HealthBarView view, 
            EnemyDamageable damageable) : base(view, damageable) { }


        public override void Initialize()
        {
            base.Initialize();
            _damageable.OnKilled += OnKilled;
            _damageable.OnDamaged += OnDisplay;
        }

        public override void Dispose()
        {
            base.Dispose();
            _damageable.OnKilled -= OnKilled;
            _damageable.OnDamaged -= OnDisplay;
        }

        private void OnDisplay(float value) => 
            Display(true);

        private void OnKilled() => 
            Display(false);
    }
}
