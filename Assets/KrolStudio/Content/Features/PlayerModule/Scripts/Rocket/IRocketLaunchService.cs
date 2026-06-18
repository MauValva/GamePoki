
namespace KrolStudio
{
    public interface IRocketLaunchService
    {
        bool TryLaunch();
        void Tick();
        void FindTarget();
        void DisableFocus();
    }
}