using UnityEngine;

namespace ithappy.Zombies_Pack.CharacterCustomizationTool.Editor
{
    [CreateAssetMenu(menuName = "Character Customization Tool/Slot Library", fileName = "SlotLibrary")]
    public class SlotLibrary : ScriptableObject
    {
        public FullBodyEntry[] FullBodyCostumes;
        public SlotEntry[] Slots;
    }
}