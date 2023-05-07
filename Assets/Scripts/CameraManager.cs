using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(Camera))]
    public class CameraManager : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            SpriteGameBoard.BoardCreated += SpriteGameBoardOnBoardCreated;
        }

        private void OnDisable()
        {
            SpriteGameBoard.BoardCreated -= SpriteGameBoardOnBoardCreated;
        }

        private void SpriteGameBoardOnBoardCreated(object sender, SpriteGameBoard.BoardCreatedEventArgs e)
        {
            _camera.orthographicSize = (e.Viewport.x + 1) * Screen.height / Screen.width * 0.5f;
        }
    }
}