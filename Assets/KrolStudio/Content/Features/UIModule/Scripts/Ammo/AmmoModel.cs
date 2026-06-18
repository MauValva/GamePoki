using System;
using UnityEngine;

namespace KrolStudio
{
    public class AmmoModel
    {
        public int Current { get; private set; }
        public int LowBulletsThreshold { get; private set; }
        public bool IsEmpty => Current <= 0;

        public event Action<int, int> OnAmmoChanged; // current, threshold
        public event Action OnAmmoEmpty;

        public void Initialize(int total, int lowThreshold)
        {
            Current = total;
            LowBulletsThreshold = lowThreshold;
            OnAmmoChanged?.Invoke(Current, LowBulletsThreshold);
        }

        public void Spend(int amount)
        {
            if (IsEmpty) return;

            Current = Mathf.Max(0, Current - amount);
            OnAmmoChanged?.Invoke(Current, LowBulletsThreshold);

            if (IsEmpty)
                OnAmmoEmpty?.Invoke();
        }

        public void Reload(int newAmount)
        {
            Current = newAmount;
            OnAmmoChanged?.Invoke(Current, LowBulletsThreshold);
        }
    }
}