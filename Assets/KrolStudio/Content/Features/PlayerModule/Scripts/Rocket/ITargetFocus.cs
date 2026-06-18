using UnityEngine;

namespace KrolStudio
{
    public interface ITargetFocus
    {
        Transform Transform { get; }
        bool IsAlive { get; }
        void DisplayFocus(bool value);
    }
}