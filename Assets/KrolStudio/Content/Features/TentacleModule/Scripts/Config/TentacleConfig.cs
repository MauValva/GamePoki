using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Entity/" + nameof(TentacleConfig),
         fileName = nameof(TentacleConfig), order = 0)]
    public class TentacleConfig : ScriptableObject, IEnemyStatsConfig
    {
        public float rotationSpeed = 5f;
        public float triggerDistance = 30f;
        public float attackInterval = 1f;
        //public LayerMask damageableTargets; // Objects that can take damage
        public LayerMask detectableTargets;   // Objects that need to be detected

        [Space]
        public float sphereCastRadius;
        public float sphereCastDistance;

        [Space]
        public int baseHealth = 20;
        public int baseDamage = 5;
        public float healthGrowth = 5;

        [Space]
        public int baseReward = 10;
        public float rewardGrowth = 0.5f;

        public float BaseHealth => baseHealth;
        public float BaseDamage => baseHealth;
        public float HealthGrowth => healthGrowth;
        public int BaseReward => baseReward;
        public float RewardGrowth => rewardGrowth;
    }
}