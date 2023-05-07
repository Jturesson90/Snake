using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClipReference audioClipReference;
        private readonly string _saveKey = $"{nameof(SoundManager)}.{nameof(Volume)}";

        private float _volume;

        public float Volume
        {
            get => _volume;
            private set
            {
                _volume = value;
                PlayerPrefs.SetFloat(_saveKey, _volume);
                PlayerPrefs.Save();
            }
        }

        private void Awake()
        {
            _volume = PlayerPrefs.GetFloat(_saveKey, 1f);
        }

        private void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
            GameManager.ScoreChanged += OnScoreChanged;
            GameManager.SnakeChangedDirection += GameManagerOnSnakeChangedDirection;
            // GameManager.SnakeTurned
            // GameManager.GameOver
        }

        private void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
            GameManager.ScoreChanged -= OnScoreChanged;
            GameManager.SnakeChangedDirection -= GameManagerOnSnakeChangedDirection;
        }

        private void GameManagerOnSnakeChangedDirection(object sender, EventArgs e)
        {
            PlaySound(audioClipReference.SnakeChangeDirection, Vector3.zero);
        }

        private void OnScoreChanged(object sender, EventArgs e)
        {
            PlaySound(audioClipReference.FoodPickup, Vector3.zero);
        }

        private void OnGameStateChanged(object sender, EventArgs e)
        {
            var gm = sender as GameManager;
            if (gm == null) return;
            if (gm.IsGameOver()) PlaySound(audioClipReference.GameOver, Vector3.zero);
        }

        private void PlaySound(IReadOnlyList<AudioClip> clips, Vector3 position, float volumeMultiplier = 1f)
        {
            AudioSource.PlayClipAtPoint(clips[Random.Range(0, clips.Count)], position,
                Volume * volumeMultiplier);
        }
    }
}