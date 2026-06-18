using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Zenject;

namespace KrolStudio
{
    public class UIScreenNavigationService : IUIScreenNavigationService, IInitializable, IDisposable
    {
        private readonly UIManager _uiManager;
        private readonly SignalBus _signalBus;

        private readonly Stack<UIScreen> _screenStack = new();
        private readonly Stack<UIScreen> _popupStack = new();

        private bool _isTransitioning;

        public UIScreenNavigationService(
            UIManager uiManager,
            SignalBus signalBus)
        {
            _uiManager = uiManager;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ShowScreenSignal>(OnShowScreen);
            _signalBus.Subscribe<ClearNavigationStackSignal>(OnClearAll);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ShowScreenSignal>(OnShowScreen);
            _signalBus.Unsubscribe<ClearNavigationStackSignal>(OnClearAll);
        }

        private void OnShowScreen(ShowScreenSignal signal)
        {
            if (_screenStack.Any(s => signal.ScreenType.IsAssignableFrom(s.GetType())))
                PopTo(signal.ScreenType, signal.Configure).Forget();
            else
                Push(signal.ScreenType, signal.Configure).Forget();
        }

        private void OnClearAll()
        {
            _ = ClearAll();
        }

        public async UniTask Push<T>(Action<T> configure = null) where T : UIScreen
        {
            if (_isTransitioning) return;
            _isTransitioning = true;

            try
            {
                var screen = _uiManager.GetScreen<T>();
                configure?.Invoke(screen);

                if (_screenStack.TryPeek(out var current))
                    await current.Hide();

                _screenStack.Push(screen);
                await screen.Show();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async UniTask Push(Type screenType, Action<UIScreen> configure = null)
        {
            if (_isTransitioning) return;
            _isTransitioning = true;

            try
            {
                var screen = _uiManager.GetScreen(screenType);
                configure?.Invoke(screen);

                if (_screenStack.TryPeek(out var current))
                    await current.Hide();

                _screenStack.Push(screen);
                await screen.Show();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async UniTask PopTo<T>(Action<T> configure = null) where T : UIScreen
        {
            if (_isTransitioning) return;
            _isTransitioning = true;

            try
            {
                var screen = _uiManager.GetScreen<T>();
                configure?.Invoke(screen);

                if (!_screenStack.Any(s => s is T))
                    return;

                // Hide all screens above T
                while (_screenStack.TryPeek(out var top) && top is not T)
                {
                    await _screenStack.Pop().Hide();
                }

                // Show T if it is the top screen
                if (_screenStack.TryPeek(out var target))
                    await target.Show();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async UniTask PopTo(Type screenType, Action<UIScreen> configure = null)
        {
            if (_isTransitioning) return;
            _isTransitioning = true;

            try
            {
                var screen = _uiManager.GetScreen(screenType);
                configure?.Invoke(screen);

                if (!_screenStack.Any(s => s.GetType() == screenType))
                    return;

                // Hide all screens above T
                while (_screenStack.TryPeek(out var top) && top.GetType() != screenType)
                {
                    await _screenStack.Pop().Hide();
                }

                // Show T if it is the top screen
                if (_screenStack.TryPeek(out var target))
                    await target.Show();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async UniTask PopAndHide()
        {
            if (_isTransitioning) return;
            if (_screenStack.Count == 0) return;

            _isTransitioning = true;

            try
            {
                await _screenStack.Pop().Hide();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async UniTask Pop()
        {
            if (_isTransitioning) return;
            if (_screenStack.Count == 0) return;

            _isTransitioning = true;

            try
            {
                await _screenStack.Pop().Hide();

                if (_screenStack.TryPeek(out var previous))
                    await previous.Show();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async UniTask Replace(Type screenType, Action<UIScreen> configure = null)
        {
            if (_isTransitioning) return;
            _isTransitioning = true;

            try
            {
                var screen = _uiManager.GetScreen(screenType);
                configure?.Invoke(screen);

                if (_screenStack.TryPop(out var current))
                    await current.Hide();

                _screenStack.Push(screen);
                await screen.Show();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async UniTask Replace<T>(Action<T> configure = null) where T : UIScreen
        {
            if (_isTransitioning) return;
            _isTransitioning = true;

            try
            {
                var screen = _uiManager.GetScreen<T>();
                configure?.Invoke(screen);

                if (_screenStack.TryPop(out var current))
                    await current.Hide();

                _screenStack.Push(screen);
                await screen.Show();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async UniTask PushPopup<T>(Action<T> configure = null) where T : UIScreen
        {
            var screen = _uiManager.GetScreen<T>();
            configure?.Invoke(screen);

            _popupStack.Push(screen);
            await screen.Show();
        }

        public async UniTask ReplacePopPopup<T>(Action<T> configure = null) where T : UIScreen
        {
            var screen = _uiManager.GetScreen<T>();
            configure?.Invoke(screen);

            if (_popupStack.TryPeek(out var current))
                await current.Hide();

            _popupStack.Push(screen);
            await screen.Show();
        }

        public async UniTask PopPopup()
        {
            if (_popupStack.Count == 0) return;

            await _popupStack.Pop().Hide();
        }

        public async UniTask ClearAll()
        {
            await ClearStack(_popupStack);
            await ClearStack(_screenStack);
        }

        public async UniTask ClearStack(Stack<UIScreen> stack)
        {
            // Copy the stack to safely call Hide/Show without conflicts (ClearNavigationStackSignal)
            var stackCopy = new Stack<UIScreen>(stack);

            stack.Clear();

            while (stackCopy.Count > 0)
            {
                await stackCopy.Pop().Hide();
            }
        }
    }
}