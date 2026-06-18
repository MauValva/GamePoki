using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/TutorialConfig")]
    public class TutorialConfig : ScriptableObject
    {
        public List<TutorialStep> Steps;
        public bool CanSkip = false;
    }
}