using System.Linq;
using ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.Character;

namespace ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.SlotValidation
{
    public class SlotToggledRule : ISlotValidationRules
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
            if (_slotExceptions.Contains(type) || !isToggled)
            {
                return;
            }

            character.Toggle(SlotType.Costume, false);
        }
    }
}