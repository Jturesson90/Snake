using System;
using System.Collections.Generic;
using JTuresson.SnakeLogic;
using UnityEngine;

namespace Scripts
{
    public class SpriteGameBoard : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer wall;
        [SerializeField] private SpriteRenderer snakeHead;
        [SerializeField] private SpriteRenderer snakeBody;

        [SerializeField] private GameObject food;

        private GameObject _food;
        private GameManager _gameManager;

        private Dictionary<string, Vector3> _gridPositions;
        private int _height;
        private List<SpriteRenderer> _snake;
        private int _width;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            if (_gameManager.SnakeData != null) Setup(_gameManager.SnakeData);
        }

        private void FixedUpdate()
        {
            if (_gameManager == null) return;
            UpdateSnake(_gameManager.SnakeData.Snake);
            UpdateFood(_gameManager.SnakeData.Food);
        }

        private void OnEnable()
        {
            GameManager.GameSetupComplete += GameControllerOnGameSetupComplete;
        }

        private void OnDisable()
        {
            GameManager.GameSetupComplete -= GameControllerOnGameSetupComplete;
        }

        public static event EventHandler<BoardCreatedEventArgs> BoardCreated;

        private void GameControllerOnGameSetupComplete(object sender, EventArgs e)
        {
            _gameManager = sender as GameManager;
            if (_gameManager != null) Setup(_gameManager.SnakeData);
        }

        private void UpdateFood(Food foodData)
        {
            if (foodData != null)
            {
                _food.gameObject.SetActive(true);
                _food.transform.localPosition = GridPositionToBoard(foodData.Position.X, foodData.Position.Y);
            }
            else
            {
                _food.gameObject.SetActive(false);
            }
        }

        private void UpdateSnake(Snake snake)
        {
            var len = snake.Body.Count;
            for (var i = 0; i < len; i++)
            {
                var b = snake.Body[i];
                if (_snake.Count <= i)
                {
                    var body = Instantiate(snakeBody, transform);
                    _snake.Add(body);
                }

                _snake[i].transform.localPosition = GridPositionToBoard(b.X, b.Y);
            }
        }

        private void Setup(ISnakeData gameData)
        {
            foreach (Transform child in transform) Destroy(child.gameObject);
            _food = Instantiate(food, transform);
            UpdateFood(gameData.Food);

            _width = gameData.Width;
            _height = gameData.Height;
            _gridPositions = UpdateGridPositions(_width, _height);
            var backgroundSr = Instantiate(background, transform);
            backgroundSr.sortingOrder = -1;
            backgroundSr.transform.localScale = new Vector3(gameData.Width, gameData.Height, 1);

            var wallSr = Instantiate(wall, transform);
            wallSr.sortingOrder = -2;
            wallSr.transform.localScale = new Vector3(gameData.Width + 0.2f, gameData.Height + 0.2f, 1);

            _snake = new List<SpriteRenderer>();

            var head = Instantiate(snakeHead, transform);
            _snake.Add(head);
            UpdateSnake(gameData.Snake);
            BoardCreated?.Invoke(this, new BoardCreatedEventArgs { Viewport = wallSr.transform.localScale });
        }

        private SpriteRenderer InstantiateHead()
        {
            var head = new GameObject("SnakeHead")
            {
                transform =
                {
                    parent = transform
                }
            };
            var spriteRenderer = head.AddComponent<SpriteRenderer>();

            return spriteRenderer;
        }

        private static Dictionary<string, Vector3> UpdateGridPositions(int width, int height)
        {
            var gridPositions = new Dictionary<string, Vector3>();
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                var xPos = -(width * 0.5f) + x + 0.5f;
                var yPos = height * 0.5f - y - 0.5f;
                gridPositions.Add($"{x}{y}", new Vector3(xPos, yPos, 0));
            }

            return gridPositions;
        }

        private Vector3 GridPositionToBoard(int x, int y)
        {
            return _gridPositions[$"{x}{y}"];
        }

        public class BoardCreatedEventArgs : EventArgs
        {
            public Vector2 Viewport { get; set; }
        }
    }
}