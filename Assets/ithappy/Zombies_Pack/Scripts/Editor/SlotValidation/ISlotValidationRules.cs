using ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.Character;

namespace ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.SlotValidation
{
    public interface ISlotValidationRules
    {
        void Validate(CustomizableCharacter character, SlotType type, bool isToggled);
    }
}