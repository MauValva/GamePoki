using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Entity/" + nameof(PlayerConfig), 
        fileName = nameof(PlayerConfig), order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [Min(0)] public int baseHealth = 50;
        [Min(0)] public float healthGrowth = 5;

        [Min(1f)] public float baseMoveSpeed = 7;
        [Min(1f)] public float startSpeed = 10;
        [Min(0f)] public float speedGrowth = 10;

        [Range(0f, 1f)] public float lowHealthThreshold = 0.3f;

        [Space]
        public PunchConfig punchSettings;
    }

    [System.Serializable]
    public class PunchConfig
    {
        [Min(0f)] public float duration = 0.2f;
        [Min(0)] public int vibrato = 0;
        [Range(0, 1)] public float elasticity = 1f;
        public List<Vector3> punches;

        public Vector3 GetRandomPunch() =>
            punches != null && punches.Count > 0 
                ? punches[Random.Range(0, punches.Count)] 
                : Vector3.zero;
    }
}