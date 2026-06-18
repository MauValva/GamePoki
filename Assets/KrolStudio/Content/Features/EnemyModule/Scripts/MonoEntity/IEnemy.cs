using UnityEngine;

namespace KrolStudio
{
    public interface IEnemy
    {
        Vector3 SpawnPosition { set; }
        void SetDefaultState();
    }
}