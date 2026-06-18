using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public interface IGameFlowService
    {
        UniTask EnterGameplay<T>() where T : UIScreen;
    }
}