using System;
using Zenject;

namespace KrolStudio
{
    public class ProgressService : IProgressService, IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;

        int currentLevel;
        int incomePurchaseCount;
        int partPurchaseCount;
        bool isTutorialCompleted;
        bool isForcedAdEnabled;

        public event Action<int> OnSetLevel;
        public event Action<int> OnSetIncomePurchase;
        public event Action<int> OnSetPartPurchase;
        public event Action<bool> OnSetTutorialShown;
        public event Action<bool> OnSetForcedAdsEnabled;

        [Inject]
        public ProgressService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ProgressDataLoadedSignal>(OnDataLoaded);
            _signalBus.Fire(new RequestProgressDataSignal());
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ProgressDataLoadedSignal>(OnDataLoaded);
        }

        private void OnDataLoaded(ProgressDataLoadedSignal signal)
        {
            currentLevel = signal.CurrentLevel;
            incomePurchaseCount = signal.IncomePurchaseCount;
            partPurchaseCount = signal.PartPurchaseCount;
            isTutorialCompleted = signal.IsTutorialCompleted;
            isForcedAdEnabled = signal.IsForcedAdEnabled;
        }

        private void FireChanged(ProgressDataFlags flags) =>
           _signalBus.Fire(new ProgressDataChangedSignal
           {
               Flags = flags,
               CurrentLevel = currentLevel,
               IncomePurchaseCount = incomePurchaseCount,
               PartPurchaseCount = partPurchaseCount,
               IsTutorialCompleted = isTutorialCompleted,
               IsForcedAdEnabled = isForcedAdEnabled,
           });

        public void SetLevel(int value)
        {
            OnSetLevel?.Invoke(value);
            currentLevel = value;
            FireChanged(ProgressDataFlags.CurrentLevel);
        }

        public void SetIncomePurchaseCount(int value)
        {
            OnSetIncomePurchase?.Invoke(value);
            incomePurchaseCount = value;
            FireChanged(ProgressDataFlags.IncomePurchaseCount);
        }

        public void SetPartPurchaseCount(int value)
        {
            OnSetPartPurchase?.Invoke(value);
            partPurchaseCount = value;
            FireChanged(ProgressDataFlags.PartPurchaseCount);
        }

        public void SetTutorialCompleted(bool value)
        {
            OnSetTutorialShown?.Invoke(value);
            isTutorialCompleted = value;
            FireChanged(ProgressDataFlags.IsTutorialShown);
        }

        public void SetForcedAdsEnabled(bool value)
        {
            OnSetForcedAdsEnabled?.Invoke(value);
            isForcedAdEnabled = value;
            FireChanged(ProgressDataFlags.IsForcedAdEnabled);
        }

        public int GetLevel() => currentLevel;
        public int GetIncomePurchaseCount() => incomePurchaseCount;
        public int GetPartPurchaseCount() => partPurchaseCount;
        public bool IsTutorialCompleted() => isTutorialCompleted;
        public bool IsForcedAdsEnabled() => isForcedAdEnabled;
    }
}
