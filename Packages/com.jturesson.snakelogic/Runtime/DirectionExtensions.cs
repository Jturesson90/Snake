using System;

namespace JTuresson.SnakeLogic
{
    internal static class DirectionExtensions
    {
        internal static Vector2Int ToVector2Int(this Direction direction)
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

        internal static Vector2Int ToOppositeDirectionVector2Int(this Direction direction)
        {
            return direction switch
            {
                Direction.North => ToVector2Int(Direction.South),
                Direction.East => ToVector2Int(Direction.West),
                Direction.South => ToVector2Int(Direction.North),
                Direction.West => ToVector2Int(Direction.East),
                Direction.None => ToVector2Int(Direction.None),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}