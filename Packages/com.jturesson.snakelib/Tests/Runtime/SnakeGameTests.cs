using System.Collections.Generic;
using NUnit.Framework;

namespace JTuresson.SnakeLib.Tests.Runtime
{
    [TestFixture]
    public class SnakeGameTests
    {
        [Test]
        public void CreateSimpleGame()
        {
            const int width = 19;
            const int height = 10;
            const Direction startDirection = Direction.East;
            const int startX = 3;
            const int startY = 3;
            const int startLength = 3;
            var expectedSnake = new List<Vector2Int>()
            {
                new(3, 3),
                new(2, 3),
                new(1, 3)
            };
            var snakeGame = new SnakeGame(width, height, startDirection, startX, startY, startLength);

            Assert.AreEqual(width, snakeGame.Width);
            Assert.AreEqual(height, snakeGame.Height);
            Assert.AreEqual(startDirection, snakeGame.SnakeHeadDirection);
            Assert.AreEqual(startX, snakeGame.SnakeHeadPositionX);
            Assert.AreEqual(startY, snakeGame.SnakeHeadPositionY);
            Assert.AreEqual(startLength, snakeGame.SnakeLength);

            CollectionAssert.AreEqual(expectedSnake, snakeGame.Snake);
        }

        [Test]
        public void SnakeCanMove()
        {
            var expectedSnake = new List<Vector2Int>()
            {
                new(0, 0),
                new(1, 0),
                new(1, 1)
            };

            var snakeGame = new SnakeGame(3, 3, Direction.East, 2, 0, 3);
            snakeGame.Update(Direction.South);
            snakeGame.Update(Direction.None);
            snakeGame.Update(Direction.West);
            snakeGame.Update(Direction.None);
            snakeGame.Update(Direction.North);
            snakeGame.Update(Direction.East);
            snakeGame.Update(Direction.North);
            snakeGame.Update(Direction.West);

            CollectionAssert.AreEqual(expectedSnake, snakeGame.Snake);
        }

        [Test]
        public void SnakeCanMoveThroughWalls()
        {
            var expectedSnake = new List<Vector2Int>()
            {
                new(2, 0),
                new(0, 0),
                new(0, 2),
            };

            var snakeGame = new SnakeGame(3, 3, Direction.East, 2, 0, 3);
            // 321
            // OOO
            // OOO
            snakeGame.Update(Direction.North);
            // 032
            // OOO
            // 0O1
            snakeGame.Update(Direction.East);
            // 003
            // OOO
            // 1O2
            snakeGame.Update(Direction.South);
            // 100
            // OOO
            // 2O3
            snakeGame.Update(Direction.West);
            // 201
            // OOO
            // 3O0
            CollectionAssert.AreEqual(expectedSnake, snakeGame.Snake);
        }

        [Test]
        public void SnakeDiesWhenCollidesWithBody()
        {
            var snakeGame = new SnakeGame(5, 5, Direction.East, 4, 0, 5);

            snakeGame.Update(Direction.South);
            snakeGame.Update(Direction.West);
            snakeGame.Update(Direction.North);
            snakeGame.Update(Direction.None);

            Assert.IsTrue(snakeGame.IsGameOver);
        }

        [Test]
        public void Spawn_Food_Will_Take_Random_Non_Taken_Position()
        {
            var expectedFoodPosition = new Vector2Int(2, 0);

            var snakeGame = new SnakeGame(3, 1, Direction.East, 1, 0, 2);
            snakeGame.SpawnFood();

            Assert.AreEqual(expectedFoodPosition, snakeGame.Food.Position);
        }

        [Test]
        public void Spawn_Food_With_No_Room_Left_Game_Over()
        {
            var snakeGame = new SnakeGame(3, 1, Direction.East, 2, 0, 3);
            snakeGame.SpawnFood();
            snakeGame.Update(Direction.None);

            Assert.IsTrue(snakeGame.IsGameOver);
        }
    }
}