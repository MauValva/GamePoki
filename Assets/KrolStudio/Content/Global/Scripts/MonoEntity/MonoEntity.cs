using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public abstract class MonoEntity : MonoBehaviour
    {
        public StateMachine StateMachine { get; private set; }

        [Inject]
        public void Construct(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void SetDefaultState() { }
        protected virtual void InitializeContext() { }

        protected virtual void Start()
        {
            InitializeContext();
        }
    }
}