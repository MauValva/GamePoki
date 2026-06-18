using UnityEngine;

namespace KrolStudio
{
    public class PlayerPrefsPersistor<T> : BasePersistor<T> where T : IDataModel
    {
        public PlayerPrefsPersistor(string playerPrefsKey, T defaultDataModel) : base(playerPrefsKey, defaultDataModel)
        {
        }

        public override void SaveData()
        {
            var json = JsonUtility.ToJson(_dataModel);
            PlayerPrefs.SetString(PlayerPrefsKey, json);
            PlayerPrefs.Save();
        }

        public override void LoadData()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKey))
            {
                var json = PlayerPrefs.GetString(PlayerPrefsKey);
                _dataModel = JsonUtility.FromJson<T>(json);
            }
        }
    }
}