using System;
using Zenject;

namespace KrolStudio
{
    public abstract class BasePersistor<T> : IDataPersistor<T, T>, IInitializable, IDisposable where T : IDataModel
    {
        public string PlayerPrefsKey { get; protected set; }

        protected T _dataModel;
        protected readonly T _defaultData;
        public event Action<T> OnDataModelUpdated; // Not used

        protected BasePersistor(string playerPrefsKey, T defaultDataModel = default)
        {
            PlayerPrefsKey = playerPrefsKey;
            _defaultData = defaultDataModel;
        }

        public virtual T GetDataModel() => _dataModel ?? _defaultData;

        public virtual void UpdateModel(T dataModel)
        {
            _dataModel = dataModel;
            OnDataModelUpdated?.Invoke(_dataModel);
        }

        protected void ResetData()
        {
            _dataModel = _defaultData;
        }

        public abstract void SaveData();
        public abstract void LoadData();

        public virtual void Initialize()
        {
            LoadData();

            if (_dataModel == null && _defaultData != null)
                _dataModel = _defaultData;
            else if (_dataModel == null)
                throw new InvalidOperationException("Data model is not initialized and no default data is provided.");
        }

        public virtual void Dispose()
        {
            SaveData();
        }
    }
}