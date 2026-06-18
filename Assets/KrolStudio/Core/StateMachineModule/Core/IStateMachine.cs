using Cysharp.Threading.Tasks;
using System;

namespace KrolStudio
{
    public interface IStateMachine
    {
        UniTask<TState> Enter<TState>() where TState : class, IState;
        UniTask<TState> Enter<TState, Param1>(Param1 param) where TState : class, IState<Param1>;
        void RegisterState<TState>(Func<TState> factory) where TState : class, IExitableState;
        public void Dispose();
    }
}