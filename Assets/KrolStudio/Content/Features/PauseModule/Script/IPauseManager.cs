
namespace KrolStudio
{
    public interface IPauseManager
    {
        bool IsPaused { get; }
        void Pause();
        void Resume();
        void TogglePause();
    }
}