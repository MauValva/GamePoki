using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KrolStudio
{
    public class UpgradeScreenView : UIScreen
    {
        [SerializeField] Button playButton;
        [SerializeField] Button addPartButton;
        [SerializeField] Button addIncomeButton;
        [Space]
        [SerializeField] TextMeshProUGUI addPartText;
        [SerializeField] TextMeshProUGUI addIncomeText;
        [SerializeField] GameObject addPartDisabled;
        [SerializeField] GameObject addIncomeDisabled;

        [Header("Tutorial")]
        [SerializeField] TextMeshProUGUI selectPartText;
        [SerializeField] Button selectPartButtom;
        [Space]
        [SerializeField] GameObject startTutorialBlocker;
        [SerializeField] GameObject incomeTutorialBlocker;
        [SerializeField] GameObject addPartTutorialBlocker;

        private PartType _partToAdd;

        public event Action PlayAction;
        public event Action AddPartAction;
        public event Action<PartType> PartSelected; // Tutotial Button
        public event Action AddIncomeAction;

        private void Awake()
        {
            playButton.onClick.AddListener(() => {
                SetInteractable(false);
                PlayAction?.Invoke();
            });

            selectPartButtom.onClick.AddListener(() => PartSelected?.Invoke(_partToAdd));
            addPartButton.onClick.AddListener(() => AddPartAction?.Invoke());
            addIncomeButton.onClick.AddListener(() => AddIncomeAction?.Invoke());
        }

        private void SetInteractable(bool value)
        {
            playButton.enabled = value;
            addPartButton.enabled = value;
            addIncomeButton.enabled = value;
        }

        private void OnDestroy()
        {
            selectPartButtom.onClick.RemoveAllListeners();
            playButton.onClick.RemoveAllListeners();
            addPartButton.onClick.RemoveAllListeners();
            addIncomeButton.onClick.RemoveAllListeners();
        }

        public void SetIncomeUpgradeCost(int value) =>
            addIncomeText.text = NumberFormatter.Format(value);

        public void SetPartCreationCost(int value)
        {
            selectPartText.text = NumberFormatter.Format(value);
            addPartText.text = NumberFormatter.Format(value);
        }

        public void SetActiveAddPart(bool value)
        {
            addPartButton.interactable = value;
            addPartDisabled.SetActive(!value);
        }

        public void SetActiveAddIncome(bool value)
        {
            addIncomeButton.interactable = value;
            addIncomeDisabled.SetActive(!value);
        }

        // Tutorial
        public void SetPartToAdd(PartType type) =>
            _partToAdd = type;

        public void DisplayAddPartTutorialButton(bool value) =>
            selectPartButtom.gameObject.SetActive(value);

        public void DisplayPlayButtonBlocker(bool value) =>
           startTutorialBlocker.SetActive(value);

        public void DisplayAddPartButtonBlocker(bool value) =>
            addPartTutorialBlocker.SetActive(value);

        public void DisplayIncomeButtonBlocker(bool value) =>
            incomeTutorialBlocker.SetActive(value);

        public override UniTask Show()
        {
            SetInteractable(true);
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