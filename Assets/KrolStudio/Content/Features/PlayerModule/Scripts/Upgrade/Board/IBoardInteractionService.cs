using UnityEngine;

namespace KrolStudio
{
    public interface IBoardInteractionService
    {
        bool TryMerge(PartInteractionHandler dragged, PartType type, int level);
        Transform GetNearestFreeCell(Vector3 position);
    }
}