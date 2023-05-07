namespace JTuresson.SnakeLogic
{
    public interface ISnakeData
    {
        public Snake Snake { get; }
        public Food Food { get; }
        public bool IsGameOver { get; }
        public int Width { get; }
        public int Height { get; }
        public int Score { get; }
        public bool SnakeChangedDirection { get; }
    }
}