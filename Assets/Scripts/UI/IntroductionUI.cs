using System;
using UnityEngine;

namespace Scripts.UI
{
    public class IntroductionUI : MonoBehaviour
    {
        [SerializeField] private GameObject background;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            Hide();
        }

        private void Start()
        {
            if (_gameManager.IsWaitingToStart()) Show();
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
            if (_gameManager.IsWaitingToStart())
                Show();
            else
                Hide();
        }

        private void Show()
        {
            background.SetActive(true);
        }

        private void Hide()
        {
            background.SetActive(false);
        }
    }
}