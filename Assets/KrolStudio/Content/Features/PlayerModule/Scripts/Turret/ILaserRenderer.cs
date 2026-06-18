
using UnityEngine;

namespace KrolStudio
{
    public interface ILaserRenderer
    {
        void SetTrajectoryPoint(Transform trajectoryPoint);
        void SetActive(bool active);
        void Tick();
    }
}