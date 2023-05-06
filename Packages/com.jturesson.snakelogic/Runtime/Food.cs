namespace JTuresson.SnakeLogic
{
    public class Food
    {
        public Food(Vector2Int pos)
        {
            Position = pos;
        }

        public Vector2Int Position { get; }
    }
}