using System;

namespace KrolStudio
{
    public struct ShowScreenSignal
    {
        public Type ScreenType { get; }
        public Action<UIScreen> Configure { get; }

        public ShowScreenSignal(Type screenType, Action<UIScreen> configure = null)
        {
            ScreenType = screenType;
            Configure = configure;
        }
    }
}