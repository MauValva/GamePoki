using UnityEngine;

namespace KrolStudio
{
    public interface IGarbageService
    {
        Transform GarbageTransform { get; }
        bool IsHovered { get; }
        void Show();
        void Hide();
        void UpdateHoverColor();
    }
}
