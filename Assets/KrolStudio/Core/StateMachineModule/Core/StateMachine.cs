using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Zenject;

namespace KrolStudio
{
    public abstract class StateMachine : IStateMachine
    {
        private readonly Dictionary<Type, Func<IExitableState>> _factories = new();
        private Dictionary<Type, IExitableState> _states = new();

        Type previousState;
        IExitableState currentState;

        ILogService logService;

        [Inject]
        void Construct(ILogService logService)
        {
            this.logService = logService;
        }

        abstract public void RegisterStates(IStatesFactory state);

        public void RegisterState<TState>(Func<TState> factory) where TState : class, IExitableState
        {
            _factories[typeof(TState)] = () => factory();
        }

        TState GetOrCreateState<TState>() where TState : class, IExitableState
        {
            var type = typeof(TState);

            // If already created — return it
            if (_states.TryGetValue(type, out var existing) && existing != null)
                return existing as TState;

            // If no factory — error
            if (!_factories.TryGetValue(type, out var factory))
            {
                logService.LogWarning($"State {type.Name} not registered");
                return null;
            }

            var state = factory();
            _states[type] = state;

            return state as TState;
        }

        public void UpdateState()
        {
            if(currentState != null && currentState is IUpdatableState updateble)
            {
                updateble.Update();
            }
        }

        public bool CheckState<TState>() => 
            currentState is TState;

        async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
        {
            if(CheckState<TState>())
                return currentState as TState;

            TState newState = GetOrCreateState<TState>();

            if (newState == null)
                return null;

            if (currentState != null)
                await currentState.Exit();

            previousState = currentState?.GetType();
            currentState = newState;

            return newState;
        }

        public bool IsCurrentState<TState>() where TState : IExitableState
        {
            return currentState is TState;
        }

        public async UniTask EnterPrevious()
        {
            if (previousState == null) return;

            if (!_states.TryGetValue(previousState, out var prevState) || prevState == null)
            {
                logService.LogWarning($"Previous state {previousState.Name} not found");
                return;
            }

            if (currentState != null)
                await currentState.Exit();

            previousState = currentState?.GetType();
            currentState = prevState;

            if (prevState is IState state)
                await state.Enter();
        }

        public async UniTask<TState> Enter<TState>() where TState : class, IState
        {
            var state = await ChangeState<TState>();

            if (state == null)
            {
                logService.LogWarning($"Failed to enter {typeof(TState)} state");
                return null;
            }

            try
            {
                await state.Enter();
                return state;
            }
            catch (Exception ex)
            {
                logService.LogError(ex.Message);
                await state.Exit();
                throw;
            }
        }

        public async UniTask<TState> Enter<TState, Param1>(Param1 param1) where TState : class, IState<Param1>
        {
            var state = await ChangeState<TState>();

            if (state == null)
            {
                logService.LogError($"Failed to enter {typeof(TState)} state");
                return null;
            }

            try
            {
                await state.Enter(param1);
                return state;
            }
            catch (Exception ex)
            {
                logService.LogError(ex.Message);
                await state.Exit();
                throw;
            }
        }

        public void Dispose()
        {
            currentState?.Exit();
            currentState = null;
            _factories.Clear();
            _states.Clear();
        }
    }
}