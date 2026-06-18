using UnityEngine;

namespace KrolStudio
{
    public class EnemyShadow : MonoBehaviour, IEnemyShadow
    {
        [SerializeField] private GameObject _shadow;

        private void OnEnable() =>
            ResetState();

        public void ResetState() =>
            _shadow.SetActive(true);

        public void Hide() =>
            _shadow.SetActive(false);
    }
}