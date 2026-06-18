using UnityEngine;

namespace KrolStudio
{

    [CreateAssetMenu(menuName = "Configurations/PoolsConfig/" + nameof(GameplayPoolsConfig),
        fileName = nameof(GameplayPoolsConfig), order = 0)]
    public class GameplayPoolsConfig : ScriptableObject
    {
        public int initialEnemyPoolCount = 40;
        public int initialProjectilePoolCount = 20;
        public int initialDamagePopupPoolCount = 10;
        public int initialEnemyKillRewardTextPoolCount = 10;
        public int initialTentaclePoolCount = 10;
        public int initialSoundEmitterPoolCount = 10;
    }
}
