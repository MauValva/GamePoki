using UnityEngine;

namespace ithappy.Zombies_Pack.CharacterCustomizationTool.Editor
{
    [CreateAssetMenu(menuName = "Character Customization Tool/Slot Group Entry", fileName = "SlotGroupEntry")]
    public class SlotGroupEntry : ScriptableObject
    {
        public GroupType Type;
        public GameObject[] Variants;
    }
}