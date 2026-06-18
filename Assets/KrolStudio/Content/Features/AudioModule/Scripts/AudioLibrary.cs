using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Audio/AudioLibrary")]
    public class AudioLibrary : ScriptableObject
    {
        [SerializeField] private List<SoundConfig> _sounds;

        private Dictionary<string, SoundConfig> _lookup;

        private void OnEnable() =>
            BuildLookup();

        public SoundConfig GetConfig(string soundName)
        {
            if (_lookup == null)
                BuildLookup();

            if (_lookup.TryGetValue(soundName, out var config))
                return config;

            Debug.LogWarning($"[AudioLibrary] Sound '{soundName}' not found.");
            return null;
        }

        private void BuildLookup()
        {
            _lookup = new Dictionary<string, SoundConfig>(_sounds.Count);

            foreach (var config in _sounds)
            {
                if (string.IsNullOrEmpty(config.SoundName))
                {
                    Debug.LogWarning($"[AudioLibrary] SoundConfig '{config.name}' has empty SoundName.", config);
                    continue;
                }

                if (!_lookup.TryAdd(config.SoundName, config))
                    Debug.LogWarning($"[AudioLibrary] Duplicate SoundName '{config.SoundName}'.", config);
            }
        }

#if UNITY_EDITOR
        private void OnValidate() =>
            BuildLookup();
#endif
    }
}