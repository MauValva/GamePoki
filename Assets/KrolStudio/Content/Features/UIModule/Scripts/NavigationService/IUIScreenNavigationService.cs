using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace KrolStudio
{
    public interface IUIScreenNavigationService
    {
        UniTask Push<T>(Action<T> configure = null) where T : UIScreen;
        UniTask Push(Type screenType, Action<UIScreen> configure = null);
        UniTask Pop();
        UniTask PopAndHide();

        UniTask Replace<T>(Action<T> configure = null) where T : UIScreen;
        UniTask Replace(Type screenType, Action<UIScreen> configure = null);

        UniTask PopTo<T>(Action<T> configure = null) where T : UIScreen;
        UniTask PopTo(Type screenType, Action<UIScreen> configure = null);

        UniTask PushPopup<T>(Action<T> configure = null) where T : UIScreen;
        UniTask PopPopup();
        UniTask ReplacePopPopup<T>(Action<T> configure = null) where T : UIScreen;

        UniTask ClearAll();
        UniTask ClearStack(Stack<UIScreen> stack);
    }
}