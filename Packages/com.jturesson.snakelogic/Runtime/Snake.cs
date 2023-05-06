using System;
using System.Collections.Generic;

namespace JTuresson.SnakeLogic
{
    public class Snake
    {
        private readonly List<Vector2Int> _body;

        public Snake(int x, int y, Direction direction, int length)
        {
            if (length < 1) throw new ArgumentException("Length of snake must be at least 1");
            HeadDirection = direction;
            _body = new List<Vector2Int>();
            for (var i = 0; i < length; i++)
                _body.Add(new Vector2Int(x, y) + i * direction.ToOppositeDirectionVector2Int());
        }

        public IReadOnlyList<Vector2Int> Body => _body;
        public Vector2Int HeadPosition => _body[0];
        public Direction HeadDirection { get; internal set; }
        public int Length => _body.Count;

        internal void Grow()
        {
            var tailPosition = Body[^1];
            _body.Add(new Vector2Int(tailPosition.X, tailPosition.Y));
        }

        internal void UpdateBodyPart(int i, Vector2Int newPos)
        {
            _body[i] = newPos;
        }
    }
}