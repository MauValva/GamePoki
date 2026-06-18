using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    // View - only UI, events, and state display
    public class RocketLauncherView : MonoBehaviour, IDisplayable
    {
        [SerializeField] private Button _launchButton;
        [SerializeField] private GameObject _reloadHolder;
        [SerializeField] private TextMeshProUGUI _reloadText;
        [SerializeField] private PopUpText _rocketLaunchFailurePrefab;

        UnityEngine.Object launchFailureMessage;

        public PopUpText LaunchFailurePrefab => _rocketLaunchFailurePrefab;
        public event Action OnLaunchClicked;

        private void OnEnable() =>
            _launchButton.onClick.AddListener(OnLaunch);

        private void OnDisable() =>
            _launchButton.onClick.RemoveListener(OnLaunch);

        private void OnLaunch() =>
            OnLaunchClicked?.Invoke();

        public void SetReloadActive(bool value) =>
            _reloadHolder.SetActive(value);

        public void SetReloadText(int seconds) =>
            _reloadText.text = seconds.ToString();

        public void SetLaunchButtonInteractable(bool value) =>
            _launchButton.interactable = value;

        public void Display(bool value)
        {
            if(this != null && gameObject != null)
                gameObject.SetActive(value);
        }

        UnityEngine.Object ShowPopUpMessage(PopUpText prefab)
        {
            PopUpText message = Instantiate(prefab, transform);
            message.OnFinalize += () => Destroy(message.gameObject);
            message.Play();
            return message;
        }

        public void ShowFailureMessage()
        {
            if (launchFailureMessage == null)
                launchFailureMessage = ShowPopUpMessage(_rocketLaunchFailurePrefab);
        }
    }
}