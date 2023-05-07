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
        [SerializeField] private SpriteRenderer food;
        private SpriteRenderer _food;

        private GameController _gameController;
        private int _height;
        private List<SpriteRenderer> _snake;
        private int _width;

        private void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
        }

        private void Start()
        {
            if (_gameController.SnakeData != null) Setup(_gameController.SnakeData);
        }

        private void FixedUpdate()
        {
            if (_gameController == null) return;
            UpdateSnake(_gameController.SnakeData.Snake);
            UpdateFood(_gameController.SnakeData.Food);
        }

        private void OnEnable()
        {
            GameController.GameSetupComplete += GameControllerOnGameSetupComplete;
        }

        private void OnDisable()
        {
            GameController.GameSetupComplete -= GameControllerOnGameSetupComplete;
        }

        private void GameControllerOnGameSetupComplete(object sender, EventArgs e)
        {
            _gameController = sender as GameController;
            if (_gameController != null) Setup(_gameController.SnakeData);
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

        private Dictionary<string, Vector3> _gridPositions;

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
            wallSr.transform.localScale = new Vector3(gameData.Width + 2, gameData.Height + 2, 1);

            _snake = new List<SpriteRenderer>();
            var head = Instantiate(snakeHead, transform);
            _snake.Add(head);
            UpdateSnake(gameData.Snake);
        }

        private static Dictionary<string, Vector3> UpdateGridPositions(int width, int height)
        {
            var gridPositions = new Dictionary<string, Vector3>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var xPos = -(width * 0.5f) + x + 0.5f;
                    var yPos = height * 0.5f - y - 0.5f;
                    gridPositions.Add($"{x}{y}", new Vector3(xPos, yPos, 0));
                }
            }

            return gridPositions;
        }

        private Vector3 GridPositionToBoard(int x, int y)
        {
            return _gridPositions[$"{x}{y}"];
        }
    }
}