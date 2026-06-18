using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public interface ITentacleStateService
    {
        UniTask EnterIdle();
        UniTask EnterDead();
        UniTask EnterAttack();
    }
}
