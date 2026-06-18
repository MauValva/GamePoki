using UnityEngine;
using System.Collections.Generic;

namespace KrolStudio
{
    public static class GameConstants
    {
        [System.Serializable]
        public class Animations
        {
            public const string TentacleDeath = "TentacleDeath";
            public const string TentacleAttack = "TentacleAttack";

            public const string EnemyMovement = "Movement";
            public const string EnemyMovementVal = "MovementVal";
            public const string EnemyHit = "Hit";
            public const string EnemyAttack = "Attack";

            public const string ScaleBullets = "ScaleBullets";
            public const string ScaleRedBullets = "ScaleRedBullets";
        }

        [System.Serializable]
        public static class Vibrations
        {
            public const int ShortVibration = 20;
            public const int LongVibration = 50;
        }
  
        public static class Sounds
        {
            public readonly static Dictionary<int, string> TurretSoundByLevel = new()
            {
                { 0, "Turret1"},
                { 1, "Turret2"},
                { 2, "Turret3"},
                { 3, "Turret4"}
            };

            public const string RunningCar = "RunningCar";
            public const string CarDamage = "CarDamage";
            public const string CarCrash = "CarCrash";
            public const string UpgradePart = "UpgradePart";

            public const string Main = "Main";

            public const string RocketExplosion = "RocketExplosion";
            public const string RocketLaunch = "RocketLaunch";

            public const string TentacleDeath = "TentacleDeath";

            public const string StickmanAttack = "StickmanAttack";
            public const string StickmanDamage = "StickmanDamage";

            public readonly static List<string> StickmanReactions = new()
            {
                    "StickmanReaction1",
                    "StickmanReaction2",
            };

            public static string GetRandomStickmanReaction() =>
                StickmanReactions[Random.Range(0, StickmanReactions.Count)];
        }
    }
}