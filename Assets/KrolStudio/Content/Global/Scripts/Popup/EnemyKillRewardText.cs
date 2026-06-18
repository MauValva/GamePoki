using System;

namespace KrolStudio
{
    public class EnemyKillRewardText : PopUpText, IPoolReturnable<EnemyKillRewardText>
    {
        public event Action<EnemyKillRewardText> OnReturned;

        void Awake()
        {
            OnFinalize += ReturnToPool;
        }

        private void OnDestroy()
        {
            OnFinalize -= ReturnToPool;
        }

        public void ReturnToPool()
        {
            OnReturned?.Invoke(this);
        }
    }
}