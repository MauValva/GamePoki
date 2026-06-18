using System;

namespace KrolStudio
{
    public interface IStorageService<TKey>
    {
        void SaveProgress(TKey key, int oldValue, int amount);
        int GetAmount(TKey key);
        void SetAmount(TKey key, int amount, object sender = null);
        void AddAmount(TKey key, int amount, object sender = null);
        bool HasEnough(TKey key, int requiredAmount);
        bool TrySpend(TKey key, int amount, object sender = null);
        void SubscribeToUpdate(TKey key, Action<int, int, object> callback);
        void UnsubscribeFromUpdate(TKey key, Action<int, int, object> callback);
    }
}