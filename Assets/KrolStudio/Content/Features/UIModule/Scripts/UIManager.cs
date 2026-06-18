using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace KrolStudio
{
    public class UIManager : IInitializable
    {
        private readonly Dictionary<Type, UIScreen> _screens;
        private readonly ILogService _logService;

        public void Initialize()
        {
            foreach (var screen in _screens)
            {
                screen.Value.Hide();
            }
        }

        [Inject]
        public UIManager(
            List<UIScreen> screens,
            ILogService logService)
        {
            _screens = screens.ToDictionary(x => x.GetType());
            _logService = logService;
        }

        private T FindScreen<T>() where T : UIScreen
        {
            if (_screens.TryGetValue(typeof(T), out var screen))
                return screen as T;

            _logService.LogError($"Screen {typeof(T)} not found!");
            return null;
        }

        public UIScreen GetScreen(Type screenType)
        {
            if (_screens.TryGetValue(screenType, out var screen))
                return screen;

            _logService.LogError($"Screen {screenType} not found!");
            return null;
        }

        public T GetScreen<T>() where T : UIScreen =>
            FindScreen<T>();

        public UniTask Show<T>() where T : UIScreen =>
            FindScreen<T>()?.Show() ?? default;

        public UniTask Hide<T>() where T : UIScreen =>
            FindScreen<T>()?.Hide() ?? default;
    }
}