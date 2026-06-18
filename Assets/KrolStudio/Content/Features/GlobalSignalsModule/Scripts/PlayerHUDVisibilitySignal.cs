
namespace KrolStudio
{
    public struct PlayerHUDVisibilitySignal
    {
        public bool IsVisible { get; }
        public PlayerHUDVisibilitySignal(bool isVisible) => IsVisible = isVisible;
    }
}