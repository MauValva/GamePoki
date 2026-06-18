using UnityEngine;

namespace KrolStudio
{
    public class TutorialHandView : MonoBehaviour
    {
        [SerializeField] private Animator _handClick;
        [SerializeField] private Animator _handTap;
        [SerializeField] private Animator _handStartClick;

        public void Show(TutorialHint hint)
        {
            HideAll();
            switch (hint)
            {
                case TutorialHint.Click:
                    _handClick.gameObject.SetActive(true);
                    _handClick.Play("HandClick", -1, 0f);
                    break;
                case TutorialHint.Tap:
                    _handTap.gameObject.SetActive(true);
                    _handTap.Play("HandTap", -1, 0f);
                    break;
                case TutorialHint.StartClick:
                    _handStartClick.gameObject.SetActive(true);
                    _handStartClick.Play("HandClick", -1, 0f);
                    break;
            }
        }

        public void Hide() => HideAll();

        private void HideAll()
        {
            _handClick.gameObject.SetActive(false);
            _handTap.gameObject.SetActive(false);
            _handStartClick.gameObject.SetActive(false);
        }
    }
}