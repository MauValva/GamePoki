
using UnityEngine;

namespace KrolStudio
{
    public interface IEnemyRagdoll
    {
        void EnableRagdoll(bool value);
        void Push(Vector3 direction, float forceMagnitude);
        void Push(Transform playerTransform, float forceMagnitude);
    }
}