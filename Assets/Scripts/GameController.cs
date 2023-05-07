using System;
using JTuresson.SnakeLogic;
using Scripts.Input;
using UnityEngine;

namespace Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameData gameData;

        private float _currentTime;
        private Direction _direction;
        private GameInput _gameInput;
        private State _gameState;
        private SnakeGame _snakeGame;
        private float _speed;
        public ISnakeData SnakeData => _snakeGame;

        private State GameState
        {
            get => _gameState;
            set
            {
                if (value == _gameState) return;
                _gameState = value;
                GameStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Awake()
        {
            _gameInput = FindObjectOfType<GameInput>();
        }

        private void Start()
        {
            SetupGame();
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (1.0f / _speed > _currentTime) return;
            _currentTime = 0;
            _snakeGame.Update(_direction);
            if (_snakeGame.Food == null)
                _snakeGame.SpawnFood();
        }

        private void OnEnable()
        {
            _gameInput.OnMovement += GameInputOnOnMovement;
        }

        private void OnDisable()
        {
            _gameInput.OnMovement -= GameInputOnOnMovement;
        }

        private void GameInputOnOnMovement(object sender, MovementEventArgs e)
        {
            _direction = e.Direction;
        }

        public static event EventHandler GameStateChanged;
        public static event EventHandler GameSetupComplete;

        private void SetupGame()
        {
            _snakeGame = new SnakeGame(gameData.Width,
                gameData.Height, gameData.StartDirection, gameData.StartPosition.x, gameData.StartPosition.y,
                gameData.StartSnakeLength);
            _speed = gameData.StartSpeed;
            GameSetupComplete?.Invoke(this, EventArgs.Empty);
        }

        private enum State
        {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }
    }
}