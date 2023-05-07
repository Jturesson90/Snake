using System;
using JTuresson.SnakeLogic;
using Scripts.Input;
using UnityEngine;

namespace Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameData gameData;
        private readonly string _highScoreKey = $"{nameof(GameManager)}.{nameof(HighScore)}";
        private const float CountdownTime = 0.5f;
        private float _countdownTimer;

        private float _currentTime;
        private Direction _direction = Direction.None;
        private GameInput _gameInput;
        private float _gamePlayingTimer;
        private State _gameState;
        private float _timeAtGameOver;
        private float _timeTo;
        private int _score;
        private SnakeGame _snakeGame;
        private float _speed;
        public ISnakeData SnakeData => _snakeGame;

        public int Score
        {
            get => _score;
            private set
            {
                if (_score == value) return;
                _score = value;
                ScoreChanged?.Invoke(this, new GameScoreChangedEventArgs { Score = _score });
            }
        }

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

        public int HighScore { get; set; }

        private void Awake()
        {
            HighScore = PlayerPrefs.GetInt(_highScoreKey, 0);
            _gameInput = FindObjectOfType<GameInput>();
            GameState = State.WaitingToStart;
        }

        private void Start()
        {
            SetupGame();
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetInt(_highScoreKey, HighScore);
            PlayerPrefs.Save();
        }

        private void Update()
        {
            switch (GameState)
            {
                case State.WaitingToStart:
                    break;
                case State.CountdownToStart:
                    _countdownTimer -= Time.deltaTime;
                    if (_countdownTimer <= 0f) GameState = State.GamePlaying;
                    break;
                case State.GamePlaying:
                    GamePlayingUpdate();
                    break;
                case State.GameOver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEnable()
        {
            _gameInput.OnMovement += GameInputOnMovement;
        }

        private void OnDisable()
        {
            _gameInput.OnMovement -= GameInputOnMovement;
        }

        public bool IsWaitingToStart()
        {
            return GameState == State.WaitingToStart;
        }

        public bool IsGameOver()
        {
            return GameState == State.GameOver;
        }

        private void GamePlayingUpdate()
        {
            _currentTime += Time.deltaTime;
            if (1.0f / _speed > _currentTime) return;
            _currentTime = 0;

            _snakeGame.Update(_direction);
            Score = _snakeGame.Score;
            if (_snakeGame.IsGameOver)
            {
                if (Score > HighScore)
                    HighScore = Score;
                GameState = State.GameOver;
                _timeAtGameOver = Time.timeSinceLevelLoad;
            }

            if (_snakeGame.SnakeChangedDirection) SnakeChangedDirection?.Invoke(this, EventArgs.Empty);

            if (_snakeGame.Food == null)
                _snakeGame.SpawnFood();
        }

        private void GameInputOnMovement(object sender, MovementEventArgs e)
        {
            switch (_gameState)
            {
                case State.WaitingToStart:
                    GameState = State.CountdownToStart;
                    _countdownTimer = CountdownTime;
                    break;
                case State.CountdownToStart:
                    break;
                case State.GamePlaying:
                    _direction = e.Direction;
                    break;
                case State.GameOver:
                    if (Time.timeSinceLevelLoad < _timeAtGameOver + CountdownTime) break;
                    SetupGame();
                    GameState = State.WaitingToStart;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static event EventHandler GameStateChanged;
        public static event EventHandler<GameScoreChangedEventArgs> ScoreChanged;
        public static event EventHandler GameSetupComplete;
        public static event EventHandler SnakeChangedDirection;

        private void SetupGame()
        {
            _snakeGame = new SnakeGame(gameData.Width,
                gameData.Height, gameData.StartDirection, gameData.StartPosition.x, gameData.StartPosition.y,
                gameData.StartSnakeLength);
            _speed = gameData.StartSpeed;
            Score = 0;
            GameSetupComplete?.Invoke(this, EventArgs.Empty);
        }

        private enum State
        {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }

        public class GameScoreChangedEventArgs : EventArgs
        {
            public int Score { get; set; }
        }
    }
}