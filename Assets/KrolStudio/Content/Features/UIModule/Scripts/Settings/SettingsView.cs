using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    // View — only UI events, no logic
    public class SettingsView : UIScreen
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Toggle _vibrate;

        public event Action ResumeAction;
        public event Action RestartAction;

        private void Awake()
        {
            _resumeButton.onClick.AddListener(() => ResumeAction?.Invoke());
            _restartButton.onClick.AddListener(() => RestartAction?.Invoke());
        }

        private void OnDestroy()
        {
            _resumeButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();
        }

        public void SetMusicView(float value) =>
            _musicSlider.value = value;
        public void SetSfxView(float value) =>
            _sfxSlider.value = value;
        public void SetVibrateView(bool value) =>
            _vibrate.isOn = value;

        public float GetMusicView() =>
            _musicSlider.value;
        public float GetSfxView() =>
            _sfxSlider.value;
        public bool GetVibrateView() =>
            _vibrate.isOn;

        public override UniTask Show()
        {
            gameObject.SetActive(true);
            return default;
        }

        public override UniTask Hide()
        {
            gameObject.SetActive(false);
            return default;
        }
    }
}
