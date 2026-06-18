using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    public class UpgradeLevelController : MonoBehaviour
    {
        private UpgradeLevelSwitcher[] levelSwitchers;
        private Dictionary<PartType, UpgradeLevelSwitcher> switchersByType;

        public UpgradeLevelSwitcher[] LevelSwitchers
        {
            get 
            {
                if (levelSwitchers == null)
                    levelSwitchers = GetComponentsInChildren<UpgradeLevelSwitcher>();
                return levelSwitchers; 
            }
        }

        private Dictionary<PartType, UpgradeLevelSwitcher> SwitchersByType
        {
            get
            {
                if (switchersByType == null)
                {
                    switchersByType = new Dictionary<PartType, UpgradeLevelSwitcher>();
                    foreach (var sw in LevelSwitchers)
                    {
                        if (!switchersByType.ContainsKey(sw.PartType))
                            switchersByType.Add(sw.PartType, sw);
                    }
                }

                return switchersByType;
            }
        }

        public int GetLevel(PartType partType)
        {
            if(SwitchersByType.TryGetValue(partType, out var switcher))
                return switcher.PlayerPartLevel;

            Debug.LogWarning($"[UpgradeLevelController] PartType {partType} not found!");
            return -1;
        }

        public void SetLevel(PartType partType, int level)
        {
            if(SwitchersByType.TryGetValue(partType, out var switcher))
                switcher.Initialize(level);
        }

        public bool CanUpgradePlayerPart(PartType partType)
        {
            if (SwitchersByType.TryGetValue(partType, out var switcher))
                return switcher.CanUpgradePlayerPart();
            return false;
        }

        public void DisplayLevelIndicators(bool value)
        {
            foreach (var part in LevelSwitchers)
                part.DisplayLevelIndicator(value);
        }

        public void PlayUpgradeEffect(PartType partType)
        {
            if (SwitchersByType.TryGetValue(partType, out var switcher))
                switcher.PlayUpgradeEffect();
        }

        public void SetBacklight(PartType partType, int partLevel, bool enable)
        {
            if (SwitchersByType.TryGetValue(partType, out var switcher))
            {
                if (switcher.PlayerPartLevel < 0)
                {
                    if (enable) switcher.EnablePreviewBacklight();
                    else switcher.DisablePreviewBacklight();
                }
                else
                {
                    if (enable) switcher.EnableBacklight(partLevel);
                    else switcher.DisableBacklight();
                }
            }
        }
    }
}

