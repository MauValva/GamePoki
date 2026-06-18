using ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.Character;

namespace ithappy.Zombies_Pack.CharacterCustomizationTool.Editor.Randomizer.Steps
{
    public interface IRandomizerStep
    {
        GroupType GroupType { get; }

        StepResult Process(int count, GroupType[] groups, CustomizableCharacter character);
    }
}