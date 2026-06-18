
namespace KrolStudio
{
    public class TutorialUI
    {
        private readonly UpgradeScreenView _view;
        private readonly HUDView _hud;

        public TutorialUI(UIManager manager, HUDView hud)
        {
            _view = manager.GetScreen<UpgradeScreenView>();
            _hud = hud;
        }

        public void ShowClickStep()
        {
            _view.SetPartToAdd(PartType.Turret);
            _view.DisplayAddPartTutorialButton(true);
            _view.DisplayAddPartButtonBlocker(false);
        }

        public void ShowTapStep()
        {
            _view.DisplayAddPartTutorialButton(false);
            _view.DisplayAddPartButtonBlocker(true);
        }

        public void ShowStartClickStep()
        {
            _view.DisplayPlayButtonBlocker(false);
        }

        public void StartTutorial()
        {
            _hud.SetPausaButtonInteractable(false);

            _view.DisplayAddPartTutorialButton(true);
            _view.DisplayAddPartButtonBlocker(true);
            _view.DisplayPlayButtonBlocker(true);
            _view.DisplayIncomeButtonBlocker(true);
        }

        public void CompleteTutorial()
        {
            _hud.SetPausaButtonInteractable(true);

            _view.DisplayAddPartTutorialButton(false);
            _view.DisplayAddPartButtonBlocker(false);
            _view.DisplayPlayButtonBlocker(false);
            _view.DisplayIncomeButtonBlocker(false);
        }
    }
}