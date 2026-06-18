using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    public class RocketPart : MonoBehaviour
    {
        [SerializeField] PartLevelController partLevel;
        [SerializeField] List<MeshRenderer> rocketsRenderer;
        [SerializeField] List<Color> rocketColors;

        private void Awake() =>
            partLevel.OnLevelChanged += LevelChanged;

        private void Start() =>
            LevelChanged();

        private void OnDestroy() =>
            partLevel.OnLevelChanged -= LevelChanged;

        private void LevelChanged()
        {
            if (partLevel.Level < 0) return;
            foreach (var renderer in rocketsRenderer)
                renderer.material.color = rocketColors[partLevel.Level];
        }
    }
}