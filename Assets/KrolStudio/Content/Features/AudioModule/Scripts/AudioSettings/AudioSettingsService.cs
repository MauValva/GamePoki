using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace KrolStudio
{
    public class AudioSettingsService : IAudioSettingsService
    {
        private readonly AudioMixer _audioMixer;

        public float _musicVolume;
        public float _sfxVolume;

        [Inject]
        public AudioSettingsService(AudioMixer audioMixer) =>
            _audioMixer = audioMixer;

        public void SetMusicVolume(float value)
        {
            _musicVolume = value;
            _audioMixer.SetFloat("Music", Mathf.Log10(value) * 20);
        }

        public void SetSfxVolume(float value)
        {
            _sfxVolume = value;
            _audioMixer.SetFloat("Sfx", Mathf.Log10(value) * 20);
        }

        public float GetMusicVolume() => _musicVolume;

        public float GetSfxVolume() => _sfxVolume;
    }
}