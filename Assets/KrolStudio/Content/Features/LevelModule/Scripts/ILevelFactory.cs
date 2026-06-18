using UnityEngine;

namespace KrolStudio
{
    public interface ILevelFactory
    {
        GameObject Spawn(Level level);
    }
}