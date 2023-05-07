using JTuresson.SnakeLogic;
using UnityEngine;
using Vector2Int = UnityEngine.Vector2Int;

namespace Scripts
{
    [CreateAssetMenu]
    public class GameData : ScriptableObject, IGameData
    {
        [SerializeField] private int width = 19;
        [SerializeField] private int height = 9;
        [SerializeField] private Vector2Int startPosition = new(5, 6);
        [SerializeField] private Direction startDirection = Direction.East;
        [SerializeField] private int startSnakeLength = 3;
        [SerializeField] private float startSpeed = 1;

        public int Width => width;
        public int Height => height;
        public Vector2Int StartPosition => startPosition;
        public Direction StartDirection => startDirection;
        public int StartSnakeLength => startSnakeLength;
        public float StartSpeed => startSpeed;
    }
}