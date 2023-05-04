using System;
using System.Collections.Generic;
using System.Linq;

namespace JTuresson.SnakeLib
{
    public class SnakeGame
    {
        public SnakeGame(int width, int height, Direction currentDirection, int startX, int startY, int startLength)
        {
            Width = width;
            Height = height;
            SnakeHeadDirection = currentDirection;
            SnakeHeadPositionX = startX;
            SnakeHeadPositionY = startY;
            Snake = new List<Vector2Int>();
            for (var i = 0; i < startLength; i++)
                Snake.Add(new Vector2Int(startX, startY) + i * GetOppositeDirection(currentDirection));
            _random = new Random();
        }

        private readonly Random _random;
        public List<Vector2Int> Snake { get; }
        public Food Food { get; private set; }
        public bool IsGameOver { get; private set; }
        public int Width { get; }
        public int Height { get; }
        public int SnakeHeadPositionX { get; }
        public int SnakeHeadPositionY { get; }
        public int SnakeLength => Snake.Count;
        public Direction SnakeHeadDirection { get; private set; }

        private static Vector2Int GetVector2IntFromDirection(Direction direction)
        {
            return direction switch
            {
                Direction.North => new Vector2Int(0, -1),
                Direction.East => new Vector2Int(1, 0),
                Direction.South => new Vector2Int(0, 1),
                Direction.West => new Vector2Int(-1, 0),
                Direction.None => new Vector2Int(0, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        private static Vector2Int GetOppositeDirection(Direction direction)
        {
            return direction switch
            {
                Direction.North => GetVector2IntFromDirection(Direction.South),
                Direction.East => GetVector2IntFromDirection(Direction.West),
                Direction.South => GetVector2IntFromDirection(Direction.North),
                Direction.West => GetVector2IntFromDirection(Direction.East),
                Direction.None => GetVector2IntFromDirection(Direction.None),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public void Update(Direction inputDirection)
        {
            if (IsGameOver) return;
            var direction = inputDirection == Direction.None ? SnakeHeadDirection : inputDirection;
            SnakeHeadDirection = direction;

            MoveBody();
            MoveHead();
            if (DidHeadCollideWithBody()) Die();
        }

        public void SpawnFood()
        {
            HashSet<Vector2Int> availableSpawnPoints = new();
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                if (Snake.Contains(new Vector2Int(x, y)) == false)
                    availableSpawnPoints.Add(new Vector2Int(x, y));
            }

            if (availableSpawnPoints.Count <= 0)
            {
                Die();
                return;
            }

            var i = _random.Next(0, availableSpawnPoints.Count);
            Food = new Food(availableSpawnPoints.ElementAt(i));
        }

        private bool DidHeadCollideWithBody()
        {
            var head = Snake[0];
            for (var i = 1; i < Snake.Count; i++)
                if (head == Snake[i])
                    return true;

            return false;
        }

        private void Die()
        {
            IsGameOver = true;
        }

        private void MoveBody()
        {
            for (var i = Snake.Count - 1; i > 0; i--) Snake[i] = Snake[i - 1];
        }

        private void MoveHead()
        {
            var newPos = Snake[0] + GetVector2IntFromDirection(SnakeHeadDirection);
            if (newPos.X < 0)
                newPos.X = Width - 1;
            else if (newPos.X >= Width)
                newPos.X = 0;
            else if (newPos.Y < 0)
                newPos.Y = Height - 1;
            else if (newPos.Y >= Height)
                newPos.Y = 0;
            Snake[0] = newPos;
        }
    }
}