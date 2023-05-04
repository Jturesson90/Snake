using System;

namespace JTuresson.SnakeLib
{
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);
        public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new(a.X * b.X, a.Y * b.Y);
        public static Vector2Int operator *(int a, Vector2Int b) => new(a * b.X, a * b.Y);
        public static Vector2Int operator *(Vector2Int a, int b) => new(a.X * b, a.Y * b);
        public static bool operator ==(Vector2Int a, Vector2Int b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2Int a, Vector2Int b) => !(a == b);

        public bool Equals(Vector2Int other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2Int other && Equals(other);
        }

        public override int GetHashCode()
        {
            var num1 = X;
            var hashCode = num1.GetHashCode();
            num1 = Y;
            var num2 = num1.GetHashCode() << 2;
            return hashCode ^ num2;
        }
    }
}