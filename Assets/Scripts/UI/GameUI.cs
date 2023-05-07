using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void OnEnable()
        {
            GameManager.ScoreChanged += GameManagerOnScoreChanged;
        }


        private void OnDisable()
        {
            GameManager.ScoreChanged -= GameManagerOnScoreChanged;
        }

        private void GameManagerOnScoreChanged(object sender, GameManager.GameScoreChangedEventArgs e)
        {
            scoreText.text = e.Score.ToString();
        }
    }
}