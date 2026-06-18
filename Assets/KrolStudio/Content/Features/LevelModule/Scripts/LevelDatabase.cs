using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Level/" + nameof(LevelDatabase),
        fileName = nameof(LevelDatabase), order = 0)]
    public class LevelDatabase : ScriptableObject
    {
        [SerializeField] private List<Level> _levels;

        public int Count => _levels.Count;

        public Level GetLevel(int index)
        {
            if (index < 0 || index >= _levels.Count)
            {
                Debug.LogWarning($"Level for {index} not found.");
                return null;
            }

            return _levels[index];
        }
    }
}