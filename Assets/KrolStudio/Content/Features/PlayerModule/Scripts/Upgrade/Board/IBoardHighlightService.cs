
namespace KrolStudio
{
    //IBoardHighlightService — highlighting details on the board
    public interface IBoardHighlightService
    {
        void HighlightBoard(PartType type, int level, bool canUpgrade);
        void ClearBoard(PartType type);
    }
}