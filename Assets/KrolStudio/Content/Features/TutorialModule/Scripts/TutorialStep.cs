using System;
using Unity.VisualScripting;

namespace KrolStudio
{
    // Tutorial step — describes what to show and which condition to wait for.
    [Serializable]
    public class TutorialStep
    {
        public string Id;
        public TutorialHint Hint;   // What it shows
        public string WaitSignal;  //    Which signal completes the step
    }

    public enum TutorialHint { Click, Tap, StartClick, None }
}