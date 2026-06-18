using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Projectile/" + nameof(ProjectileConfig),
        fileName = nameof(ProjectileConfig), order = 0)]
    public class ProjectileConfig : ScriptableObject
    {
        public float flyTime = 10f;             // projectile lifetime (in seconds)
        public float flySpeed = 15f;
        public float sphereCastRadius = 0.1f;   // radius for collision check (SphereCast)
        public float hitForce = 25f;

        [Space]
        public int baseDamage = 5;              // base damage at level 1
        public float damageGrowth = 5;
    }
}