using System;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class GameOverIO : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private TextMeshProUGUI scoreText;
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            Hide();
        }

        private void Start()
        {
            if (_gameManager.IsGameOver()) Show();
        }

        private void OnEnable()
        {
            GameManager.GameStateChanged += GameManagerOnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.GameStateChanged -= GameManagerOnGameStateChanged;
        }

        private void GameManagerOnGameStateChanged(object sender, EventArgs e)
        {
            if (_gameManager.IsGameOver())
                Show();
            else
                Hide();
        }

        private void Show()
        {
            background.SetActive(true);
            scoreText.text = $"High Score: {_gameManager.HighScore}<br>Score: {_gameManager.Score}";
        }

        private void Hide()
        {
            background.SetActive(false);
        }
    }
}