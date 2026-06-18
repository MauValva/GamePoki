using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Core.InputModule
{
    public class InputListener : IInputListener, IInitializable, IDisposable
    {
        private const string PLAYER_ACTION_MAP = "Player";
        private const string LEFTBUTTON_ACTION = "LeftButton";
        private const string MOUSEPOSITION_ACTION = "MousePosition";
        private const string MONEY_ACTION = "Money";

        private readonly InputActionAsset _inputActions;
        
        private InputAction _leftButtonAction;
        private InputAction _mousePosAction;
        private InputAction _moneyAction;

        public event Action<Vector2> OnLeftButtonPerformed;
        public event Action<Vector2> OnLeftButtonStarted;
        public event Action<Vector2> OnLeftButtonCanceled;

        public event Action<Vector2> OnMousePositionPerformed;

        public event Action OnMoneyPerformed;
        public event Action OnMoneyStarted;
        public event Action OnMoneyCanceled;

        public Vector2 CurrentMousePosition => 
            _mousePosAction.ReadValue<Vector2>();

        [Inject]
        public InputListener(InputActionAsset inputActionAsset)
        {
            _inputActions = inputActionAsset;
        }

        public void Initialize()
        {
            _inputActions.Enable();

            _leftButtonAction = _inputActions.FindActionMap(PLAYER_ACTION_MAP).FindAction(LEFTBUTTON_ACTION);
            _leftButtonAction.performed += OnInteraction;
            _leftButtonAction.started += OnInteraction;
            _leftButtonAction.canceled += OnInteraction;

            _mousePosAction = _inputActions.FindActionMap(PLAYER_ACTION_MAP).FindAction(MOUSEPOSITION_ACTION);
            _mousePosAction.performed += OnMousePosition;

            _moneyAction = _inputActions.FindActionMap(PLAYER_ACTION_MAP).FindAction(MONEY_ACTION);
            _moneyAction.performed += OnMoney;
            _moneyAction.started += OnMoney;
            _moneyAction.canceled += OnMoney;
        }

        private void OnMousePosition(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnMousePositionPerformed?.Invoke(context.ReadValue<Vector2>());
        }

        private void OnMoney(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnMoneyPerformed?.Invoke();

            if (context.started)
                OnMoneyStarted?.Invoke();

            if (context.canceled)
                OnMoneyCanceled?.Invoke();
        }

        public void Dispose()
        {
            _inputActions.Disable();

            _leftButtonAction.performed -= OnInteraction;
            _leftButtonAction.started -= OnInteraction;
            _leftButtonAction.canceled -= OnInteraction;

            _mousePosAction.performed -= OnMousePosition;
        }

        public void OnInteraction(InputAction.CallbackContext context)
        {
            Vector2 pos = _mousePosAction.ReadValue<Vector2>();

            if (context.performed)
                OnLeftButtonPerformed?.Invoke(pos);

            if (context.started)
                OnLeftButtonStarted?.Invoke(pos);

            if (context.canceled)
                OnLeftButtonCanceled?.Invoke(pos);
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}