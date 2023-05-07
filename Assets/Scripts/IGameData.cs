using JTuresson.SnakeLogic;
using Vector2Int = UnityEngine.Vector2Int;

namespace Scripts
{
    public interface IGameData
    {
        int Width { get; }
        int Height { get; }
        Vector2Int StartPosition { get; }
        Direction StartDirection { get; }
        int StartSnakeLength { get; }
        float StartSpeed { get; }
    }
}