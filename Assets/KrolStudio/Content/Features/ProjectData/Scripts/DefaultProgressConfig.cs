using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Progress/" + nameof(DefaultProgressConfig),
       fileName = nameof(DefaultProgressConfig), order = 0)]
    public class DefaultProgressConfig : ScriptableObject
    {
        public List<CurrencySettings> currencySettings;
        public List<PartTypeSettings> partSettings;

        [Header("Settings")]
        public bool vibrate = false;
        [Range(0.001f, 1f)] public float musicVolume = 0.5f;
        [Range(0.001f, 1f)] public float sfxVolume = 0.5f;

        [Header("Settings")]
        public bool isForcedAdEnabled = false;
    }

    [System.Serializable]
    public class PartTypeSettings
    {
        public PartType type;
        [Range(-1, 10)] public int level;
    }

    [System.Serializable]
    public class CurrencySettings
    {
        public CurrencyType type;
        [Min(0)] public int count;
    }
}