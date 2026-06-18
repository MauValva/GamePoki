using UnityEngine.Audio;
using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Audio/SoundConfig")]
    public class SoundConfig : ScriptableObject
    {
        public string SoundName;
        public AudioClip Clip;

        [Range(0f, 1f)] public float Volume = 1f;
        [Range(-3f, 3f)] public float Pitch = 1f;
        public bool Loop = false;

        [Header("Mixer")]
        public AudioMixerGroup MixerGroup;

        [Header("3D Settings")]
        [Tooltip("0 = 2D, 1 = 3D")]
        [Range(0f, 1f)] public float SpatialBlend = 1f;
        public float MinDistance = 1f;
        public float MaxDistance = 500f;
    }
}