using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public interface IPlayerStateService
    {
        UniTask EnterUpgrade();
        UniTask EnterIdle();
        UniTask EnterRun();
        UniTask EnterFinish();
        UniTask EnterDead();
    }
}