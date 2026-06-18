using System;
using UnityEngine;

namespace Core.InputModule
{
    public interface IInputListener : DefaultInputActions2.IPlayerActions, DefaultInputActions2.IUIActions
    {
        public event Action<Vector2> OnLeftButtonPerformed;
        public event Action<Vector2> OnLeftButtonStarted;
        public event Action<Vector2> OnLeftButtonCanceled;

        public event Action<Vector2> OnMousePositionPerformed;

        public event Action OnMoneyPerformed;
        public event Action OnMoneyStarted;
        public event Action OnMoneyCanceled;

        Vector2 CurrentMousePosition { get; }
    }
}