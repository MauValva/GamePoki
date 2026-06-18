using System;
using UnityEngine;

namespace KrolStudio
{
    public class MonoDamageable : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _maxHealth;
        [SerializeField] private DamageableType _damageableType;

        private bool _isDead;

        public event Action OnKilled;
        public event Action<float> OnDamaged;
        public event Action<float, float> OnHealthChanged;
        public event Action<HitData> OnHitReceived;

        public DamageableType DamageableType => _damageableType;
        public bool IsActive => !_isDead;

        public float Ratio => CurrentHealth / MaxHealth;

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                _currentHealth = Mathf.Min(_currentHealth, _maxHealth);

                OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
            }
        }

        private void OnEnable() =>
            _isDead = false;

        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value > _maxHealth ? _maxHealth : value;
        }

        public void Damage(float damage)
        {
            if (_isDead || damage <= 0f)
                return;

            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
            
            OnDamaged?.Invoke(damage);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
            
            if (_currentHealth > 0)
                return;

            _isDead = true;
            Destroyed();
        }

        public void Damage(HitData hitData)
        {
            OnHitReceived?.Invoke(hitData);
            Damage(hitData.Damage);
        }

        public void Heal(float amount)
        {
            if (_isDead || amount <= 0f)
                return;

            _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void SetHealth(float health)
        {
            CurrentHealth = health;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void Destroyed()
        {
            OnKilled?.Invoke();
            //Destroy(gameObject);
        }
    }
}