using UnityEngine;

namespace KrolStudio
{
    public interface ICullable
    {
        bool IsCulled { get; }
        bool ShouldCull(Vector3 camPos, Plane[] frustumPlanes, CullingConfig config);
        void Cull();
        void Restore();
    }
}