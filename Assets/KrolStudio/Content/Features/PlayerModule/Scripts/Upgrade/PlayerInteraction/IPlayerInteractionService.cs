
namespace KrolStudio
{
    public interface IPlayerInteractionService
    {
        bool TryUpgrade(PartType type, int level);
        void ShowBacklight(PartType type, int level);
        void ClearBacklight(PartType type);
    }
}