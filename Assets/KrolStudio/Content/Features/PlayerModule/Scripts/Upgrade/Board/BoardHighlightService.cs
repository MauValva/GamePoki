using Zenject;

namespace KrolStudio
{
    public class BoardHighlightService : IBoardHighlightService
    {
        private readonly BoardView _boardView;

        [Inject]
        public BoardHighlightService(BoardView boardView) =>
            _boardView = boardView;

        public void HighlightBoard(PartType type, int level, bool canUpgrade)
        {
            foreach (var part in _boardView.GetAllParts())
            {
                if (part.PartType != type) continue;

                bool isGreen = part.Level == level && canUpgrade;
                part.EnableBacklight(isGreen);
            }
        }

        public void ClearBoard(PartType type)
        {
            foreach (var part in _boardView.GetAllParts())
            {
                if (part.PartType == type)
                    part.DisableBacklight();
            }
        }
    }
}