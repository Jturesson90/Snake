using System;
using System.Collections.Generic;
using System.Linq;

namespace JTuresson.SnakeLogic
{
    public class SnakeGame : ISnakeData
    {
        private readonly Random _random;

        public SnakeGame(int width, int height, Direction currentDirection, int startX, int startY, int startLength)
        {
            Width = width;
            Height = height;
            Snake = new Snake(startX, startY, currentDirection, startLength);
            _random = new Random();
        }

        public Snake Snake { get; }
        public Food Food { get; private set; }
        public bool IsGameOver { get; private set; }
        public int Width { get; }
        public int Height { get; }
        public int Score { get; private set; }

        public void Update(Direction inputDirection)
        {
            if (IsGameOver) return;
            var direction = inputDirection == Direction.None ? Snake.HeadDirection : inputDirection;
            Snake.HeadDirection = direction;

            MoveBody();
            MoveHead();
            if (DidHeadCollideWithBody()) Die();
            CheckForFood();
        }

        public void SpawnFood()
        {
            HashSet<Vector2Int> availableSpawnPoints = new();
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (Snake.Body.Contains(new Vector2Int(x, y)) == false)
                    availableSpawnPoints.Add(new Vector2Int(x, y));

            if (availableSpawnPoints.Count <= 0)
            {
                Die();
                return;
            }

            var i = _random.Next(0, availableSpawnPoints.Count);
            Food = new Food(availableSpawnPoints.ElementAt(i));
        }

        private void CheckForFood()
        {
            if (Food == null || Snake.HeadPosition != Food.Position) return;
            Score++;
            Snake.Grow();
            Food = null;
        }


        private bool DidHeadCollideWithBody()
        {
            var head = Snake.Body[0];
            var snakeLength = Snake.Body.Count;
            for (var i = 1; i < snakeLength; i++)
                if (head == Snake.Body[i])
                    return true;

            return false;
        }

        private void Die()
        {
            IsGameOver = true;
        }

        private void MoveBody()
        {
            for (var i = Snake.Body.Count - 1; i > 0; i--) Snake.UpdateBodyPart(i, Snake.Body[i - 1]);
        }

        private void MoveHead()
        {
            var newPos = Snake.Body[0] + Snake.HeadDirection.ToVector2Int();
            if (newPos.X < 0)
                newPos.X = Width - 1;
            else if (newPos.X >= Width)
                newPos.X = 0;
            else if (newPos.Y < 0)
                newPos.Y = Height - 1;
            else if (newPos.Y >= Height)
                newPos.Y = 0;
            Snake.UpdateBodyPart(0, newPos);
        }
    }
}