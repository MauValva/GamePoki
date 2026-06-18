using System.Linq;
using ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.Character;

namespace ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.SlotValidation
{
    public class FullBodyToggledRule : ISlotValidationRules
    {
        private readonly SlotType[] _slotExceptions =
        {
            SlotType.Costume,
            SlotType.Body,
            SlotType.Head,
            SlotType.Eyes,
            SlotType.Eyelids,
        };

        public void Validate(CustomizableCharacter character, SlotType type, bool isToggled)
        {
            if (type != SlotType.Costume || !isToggled)
            {
                return;
            }

            foreach (var slot in character.Slots.Where(s => !_slotExceptions.Contains(s.Type)))
            {
                slot.Toggle(false);
            }
        }
    }
}