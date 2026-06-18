using UnityEngine;

namespace ithappy.Zombies_Pack.CharacterCustomizationTool.Editor
{
    [CreateAssetMenu(menuName = "Character Customization Tool/Slot Entry", fileName = "SlotEntry")]
    public class SlotEntry : ScriptableObject
    {
        public SlotType Type;
        public SlotGroupEntry[] Groups;
    }
}