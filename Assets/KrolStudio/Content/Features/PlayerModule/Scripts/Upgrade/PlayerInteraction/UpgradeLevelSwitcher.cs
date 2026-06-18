using UnityEngine;

namespace KrolStudio
{
    [RequireComponent(typeof(PartLevelController), typeof(PartVisualsController))]
    public class UpgradeLevelSwitcher : MonoBehaviour
    {
        [SerializeField] PartType partType;

        private PartLevelController _levelController;
        private PartVisualsController _visualsController;

        public PartType PartType => partType;

        private PartLevelController LevelController
        {
            get
            {
                if (_levelController == null)
                    _levelController = GetComponent<PartLevelController>();
                return _levelController;
            }
        }

        private PartVisualsController VisualsController
        {
            get
            {
                if (_visualsController == null)
                    _visualsController = GetComponent<PartVisualsController>();
                return _visualsController;
            }
        }

        public int PlayerPartLevel => LevelController.Level;

        public void Initialize(int level) 
        {
            LevelController.Initialize(partType, level);

            VisualsController.Initialize(LevelController.FindCurrentPart(), level);
            VisualsController.UpdateLevelIndicator(LevelController.Level, LevelController.CanUpgrade());
            VisualsController.DisplayLevelIndicator(!(LevelController.Level < 0));
        }

        public bool CanUpgradePlayerPart() =>
            LevelController.CanUpgrade();

        public void PlayUpgradeEffect() =>
            VisualsController.PlayEffect();

        public void EnablePreviewBacklight() =>
            VisualsController.EnablePreviewBacklight();

        public void DisablePreviewBacklight() =>
            VisualsController.DisablePreviewBacklight();

        public void EnableBacklight(int partLevel) =>
            VisualsController.EnableBacklight(partLevel >= LevelController.Level && LevelController.CanUpgrade());

        public void DisableBacklight() =>
            VisualsController.DisableBacklight();

        public void DisplayLevelIndicator(bool value) =>
            VisualsController.DisplayLevelIndicator(value);
    }
}