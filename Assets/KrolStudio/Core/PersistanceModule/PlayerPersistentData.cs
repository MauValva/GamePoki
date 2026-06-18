using System;

namespace KrolStudio
{
    [Serializable]
    public class PlayerPersistentData : IDataModel
    {
        public float MaxHealth;
        public float CurrentHealth;
        public float Currency;
        
        public PlayerPersistentData(float maxHealth, float currentHealth, float currency)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            Currency = currency;
        }

        public PlayerPersistentData()
        {
        }
    }
}