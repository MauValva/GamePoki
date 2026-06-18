using System.Collections.Generic;
using System;

namespace KrolStudio
{
    [Serializable]
    public class WalletData : IDataModel
    {
        public List<CurrencySettings> Entries = new();

        public WalletData(List<CurrencySettings> entries)
        {
            Entries = entries;
        }
    }

    [Serializable]
    public class WalletEntry
    {
        public CurrencyType currency;
        public int amount;
    }
}