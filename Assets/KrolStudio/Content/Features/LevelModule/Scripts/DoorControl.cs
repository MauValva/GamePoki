using UnityEngine;

namespace KrolStudio
{
    public class DoorControl : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [TagSelector] public string[] validTags;

        bool isOpen = false;

        void Start()
        {
            animator.SetTrigger("Close");
        }

        public void Interaction()
        {
            isOpen = !isOpen;
            animator.SetTrigger(isOpen ? "Open" : "Close");
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (string tag in validTags)
            {
                if (other.CompareTag(tag))
                {
                    Interaction();
                    break;
                }
            }
        }
    }
}