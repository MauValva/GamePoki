using ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.Character;

namespace ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.SlotValidation
{
    public class SlotValidator
    {
        private readonly ISlotValidationRules[] _slotValidationRules =
        {
            new FullBodyToggledRule(),
            new SlotToggledRule(),
            new OverallToggleRule(),
        };

        public void Validate(CustomizableCharacter character, SlotType type, bool isToggled)
        {
            foreach (var rule in _slotValidationRules)
            {
                rule.Validate(character, type, isToggled);
            }
        }
    }
}