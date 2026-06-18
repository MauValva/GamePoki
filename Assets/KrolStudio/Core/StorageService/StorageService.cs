using System.Collections.Generic;
using System;
using Zenject;

namespace KrolStudio
{
    public abstract class StorageService<TKey> : IStorageService<TKey>, IInitializable
    {
        private Dictionary<TKey, int> _storage = new();
        private Dictionary<TKey, Action<int, int, object>> _onUpdateByType = new();

        abstract public void SaveProgress(TKey key, int oldValue, int amount);

        public virtual void Initialize()
        {
            _storage.Clear();
            foreach (TKey key in Enum.GetValues(typeof(TKey)))
                _storage[key] = 0;
        }

        private Dictionary<TKey, int> GetStorage() => _storage;

        public int GetAmount(TKey key)
        {
            _storage.TryGetValue(key, out int value);
            return value;
        }

        public void SetAmount(TKey key, int amount, object sender = null)
        {
            int oldValue = _storage[key];
            if (oldValue == amount) return;

            _storage[key] = amount;

            if (_onUpdateByType.TryGetValue(key, out var callback))
                callback?.Invoke(oldValue, amount, sender);

            SaveProgress(key, oldValue, amount);
        }

        public void AddAmount(TKey key, int amount, object sender = null) =>
            SetAmount(key, GetAmount(key) + amount, sender);

        public bool HasEnough(TKey key, int requiredAmount) =>
            GetAmount(key) >= requiredAmount;

        public bool TrySpend(TKey key, int amount, object sender = null)
        {
            if (!HasEnough(key, amount)) return false;
            SetAmount(key, GetAmount(key) - amount, sender);
            return true;
        }

        public void SubscribeToUpdate(TKey key, Action<int, int, object> callback)
        {
            if (_onUpdateByType.ContainsKey(key))
                _onUpdateByType[key] += callback;
            else
                _onUpdateByType[key] = callback;
        }

        public void UnsubscribeFromUpdate(TKey key, Action<int, int, object> callback)
        {
            if (_onUpdateByType.TryGetValue(key, out var action))
            {
                action -= callback;
                if (action == null)
                    _onUpdateByType.Remove(key);
                else
                    _onUpdateByType[key] = action;
            }
        }
    }
}