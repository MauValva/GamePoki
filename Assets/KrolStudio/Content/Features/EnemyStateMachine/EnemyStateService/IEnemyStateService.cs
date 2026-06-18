using Cysharp.Threading.Tasks;

namespace KrolStudio
{
    public interface IEnemyStateService
    {
        UniTask EnterIdle();
        UniTask EnterChase();
        UniTask EnterDead();
        UniTask EnterAttack();
        UniTask EnterHit();
        UniTask EnterInactive();
        UniTask EnterWander();
        UniTask EnterPrevious();
    }
}
