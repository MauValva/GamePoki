using System;

namespace KrolStudio
{
    public interface IDamageable
    {
        DamageableType DamageableType { get; }
        bool IsActive { get; }
        float MaxHealth { get; set; }
        float CurrentHealth { get; set; }
        float Ratio { get; }

        event Action<float, float> OnHealthChanged;
        event Action<float> OnDamaged;
        event Action OnKilled;
        event Action<HitData> OnHitReceived;

        void Damage(float damage);
        void Damage(HitData hitData);

        void SetHealth(float health);
        void Heal(float amount);
        void Destroyed();
    }
}