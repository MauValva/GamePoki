using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    public class CompleteView : UIScreen
    {
        [SerializeField] Transform rewardHolder;
        [SerializeField] Image rewardIconPrefab;
        [SerializeField] TextMeshProUGUI rewardMessage;
        [SerializeField] Button nextButton;

        public event Action NextAction;
        public event Action InitReward;

        List<Image> rewardIcons = new();

        private void Awake()
        {
            nextButton.onClick.AddListener(() => NextAction?.Invoke());
        }

        private void OnDestroy()
        {
            nextButton.onClick.RemoveAllListeners();
        }

        public void SetRewardsView(List<Sprite> icons)
        {
            RemoveRewardsView();

            foreach (var sprite in icons)
            {
                rewardIcons.Add(Instantiate(rewardIconPrefab, rewardHolder));
                rewardIcons[^1].sprite = sprite;
            }
        }

        private void RemoveRewardsView()
        {
            foreach (var icon in rewardIcons)
                Destroy(icon.gameObject);
            rewardIcons.Clear();
        }

        public void SetRewardMessage(string message) =>
            rewardMessage.text = message;


        public override async UniTask Show()
        {
            InitReward?.Invoke();
            gameObject.SetActive(true);
        }

        public override async UniTask Hide()
        {
            gameObject.SetActive(false);
        }
    }
}