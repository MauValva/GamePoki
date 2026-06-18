using Core.InputModule;
using System;
using System.Linq;
using Zenject;

namespace KrolStudio
{
    public class WalletService : StorageService<CurrencyType>, IWalletService, IDisposable
    {
        private readonly IInputListener _inputListener;
        private readonly SignalBus _signalBus;

        [Inject]
        public WalletService(
            IInputListener inputListener, 
            SignalBus signalBus)
        {
            _inputListener = inputListener;
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            base.Initialize();
            _inputListener.OnMoneyPerformed += AddMoney;

            _signalBus.Subscribe<WalletLoadedSignal>(OnDataLoaded);
            _signalBus.Fire(new RequestWalletSignal());
        }

        public void Dispose()
        {
            _inputListener.OnMoneyPerformed -= AddMoney;

            _signalBus.Unsubscribe<WalletLoadedSignal>(OnDataLoaded);
        }

        private void OnDataLoaded(WalletLoadedSignal signal)
        {
            foreach (var item in signal.Entries)
                SetAmount(item.type, item.count);
        }

        private void AddMoney() =>
            AddAmount(CurrencyType.Coin, 1000000);

        public bool TryPurchase(CurrencyType currency, int price, object sender)
        {
            if (!HasEnough(currency, price))
                return false;

            SetAmount(currency, GetAmount(currency) - price, sender);
            return true;
        }

        private void FireChanged() =>
           _signalBus.Fire(new WalletChangedSignal
           {
               Entries = Enum.GetValues(typeof(CurrencyType))
                            .Cast<CurrencyType>()
                            //.Where(t => t != CurrencyType.None)
                            .Select(c => new CurrencySettings
                            {
                                type = c,
                                count = GetAmount(c)
                            })
                            .ToList()
           });

        public override void SaveProgress(CurrencyType key, int oldValue, int amount)
        {
            FireChanged();
        }
    }
}
