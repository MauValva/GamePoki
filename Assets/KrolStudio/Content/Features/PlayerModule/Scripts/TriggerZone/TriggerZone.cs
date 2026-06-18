using UnityEngine;

namespace KrolStudio
{
    public class TriggerZone : MonoBehaviour
    {
        [TagSelector] 
        [SerializeField] 
        private string _targetTag;

        private ITriggerAction[] _actions;

        private void Awake() =>
            _actions = GetComponents<ITriggerAction>();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_targetTag)) return;
            
            foreach (var action in _actions)
                action.Execute();
        }
    }
}