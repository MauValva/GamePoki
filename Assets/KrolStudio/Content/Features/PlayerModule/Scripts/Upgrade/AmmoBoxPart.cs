using TMPro;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class AmmoBoxPart : MonoBehaviour
    {
        [SerializeField] PartLevelController partLevel;
        [SerializeField] TMP_Text bulletAmount;

        private PlayerStatsCalculator _statsCalculator;

        [Inject]
        void Construct(PlayerStatsCalculator statsCalculator)
        {
            _statsCalculator = statsCalculator;
        }

        private void Awake() =>
            partLevel.OnLevelChanged += LevelChanged;

        private void Start() =>
            LevelChanged();

        private void OnDestroy() =>
            partLevel.OnLevelChanged -= LevelChanged;

        private void LevelChanged() =>
            bulletAmount.text = _statsCalculator.GetAmmo(partLevel.Level).ToString();

    }
}