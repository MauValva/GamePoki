using UnityEngine;

namespace KrolStudio
{
    public class TargetFocus : MonoBehaviour, ITargetFocus
    {
        [SerializeField] GameObject centerFocus;
        
        public Transform Transform => transform;

        private IDamageable _damageable;

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
            _damageable.OnKilled += OnKilled;
            DisplayFocus(false);
        }

        private void OnDestroy()
        {
            _damageable.OnKilled -= OnKilled;
        }

        private void OnKilled() =>
          DisplayFocus(false);

        public bool IsAlive => _damageable.IsActive;

        public void DisplayFocus(bool value) =>
            centerFocus.SetActive(value);
    }
}