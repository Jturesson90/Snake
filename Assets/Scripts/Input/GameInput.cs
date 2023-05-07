using System;
using JTuresson.SnakeLogic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Input
{
    public class GameInput : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;

        public event EventHandler<MovementEventArgs> OnMovement;

        private MovementEventArgs _upArgs;
        private MovementEventArgs _downArgs;
        private MovementEventArgs _leftArgs;
        private MovementEventArgs _rightArgs;

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
            _upArgs = new MovementEventArgs() { Direction = Direction.North };
            _downArgs = new MovementEventArgs() { Direction = Direction.South };
            _leftArgs = new MovementEventArgs() { Direction = Direction.West };
            _rightArgs = new MovementEventArgs() { Direction = Direction.East };
        }

        private void OnEnable()
        {
            _playerInputActions.Player.Up.performed += OnUp;
            _playerInputActions.Player.Down.performed += OnDown;
            _playerInputActions.Player.Left.performed += OnLeft;
            _playerInputActions.Player.Right.performed += OnRight;
        }

        private void OnDisable()
        {
            _playerInputActions.Player.Up.performed -= OnUp;
            _playerInputActions.Player.Down.performed -= OnDown;
            _playerInputActions.Player.Left.performed -= OnLeft;
            _playerInputActions.Player.Right.performed -= OnRight;
        }

        private void OnUp(InputAction.CallbackContext obj)
        {
            OnMovement?.Invoke(this, _upArgs);
        }

        private void OnDown(InputAction.CallbackContext obj)
        {
            OnMovement?.Invoke(this, _downArgs);
        }

        private void OnLeft(InputAction.CallbackContext obj)
        {
            OnMovement?.Invoke(this, _leftArgs);
        }

        private void OnRight(InputAction.CallbackContext obj)
        {
            OnMovement?.Invoke(this, _rightArgs);
        }
    }

    public class MovementEventArgs : EventArgs
    {
        public Direction Direction { get; set; }
    }
}