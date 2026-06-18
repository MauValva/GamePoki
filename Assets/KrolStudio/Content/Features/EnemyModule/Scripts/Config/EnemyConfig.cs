using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Entity/" + nameof(EnemyConfig),
         fileName = nameof(EnemyConfig), order = 0)]
    public class EnemyConfig : ScriptableObject, IEnemyStatsConfig
    {
        public int baseHealth = 10;
        public int baseDamage = 5;
        public int healthGrowth = 5;

        [Space]
        public int baseReward = 10;
        public float rewardGrowth = 0.5f;

        [Space]
        public float speed = 3.5f;
        public float rotationSpeed = 3f;
        public float walkingSpeed = 2f;

        [Space]
        public float attackDistance = 0.5f;
        public float triggerDistance = 30;
        public float chaseStopDistance = 3;
        public float wanderArrivalDistance = 0.1f;
        public float behindAttackDistance = 1f;
        public float backAngleThreshold = 0.1f;

        [Space]
        public float walkingThreshold = 1f;   // check BlendThree 
        public float runningThreshold = 2f;   // check BlendThree 
        public float accelerationMultiplier = 3f;

        [Space]
        public float pushRadius = 1f;
        public float hitForce = 5;

        [Space]
        public Vector2 idleDuration = new Vector2(1.5f, 3f);
        public float wanderZoneRadius = 5f;

        public float BaseHealth => baseHealth;
        public float BaseDamage => baseDamage;
        public float HealthGrowth => healthGrowth;
        public int BaseReward => baseReward;
        public float RewardGrowth => rewardGrowth;
    }
}