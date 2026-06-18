using Zenject;

namespace KrolStudio
{
    public class TutorialStepHandler
    {
        private TutorialUI _ui;

        [Inject]
        private void Construct(TutorialUI ui)
        {
            _ui = ui;
        }

        public void Show(TutorialHint hint)
        {
            switch (hint)
            {
                case TutorialHint.Click:
                    _ui.ShowClickStep();
                    break;

                case TutorialHint.Tap:
                    _ui.ShowTapStep();
                    break;

                case TutorialHint.StartClick:
                    _ui.ShowStartClickStep();
                    break;
            }
        }

        public void StartTutorial() => _ui.StartTutorial();

        public void CompleteTutorial() => _ui.CompleteTutorial();
    }
}