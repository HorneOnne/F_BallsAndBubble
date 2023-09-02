using UnityEngine;
using System.Collections.Generic;

namespace BallsAndBubble
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static event System.Action OnScoreUp;

        // SCORE & BEST
        private int _score;
        private int _record;

        public enum GamePlayType
        {
            CLASSIC,
            SURVIVAL,
        }
        public GamePlayType PlayType;

        #region Properties
        public int Score { get => _score; }
        public int Record { get => _record; }
        #endregion


        private void Awake()
        {
            // Check if an instance already exists, and destroy the duplicate
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // FPS
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            // Make the GameObject persist across scenes
            DontDestroyOnLoad(this.gameObject);
        }

        public void SetRecord(int score)
        {
            if (_record < score)
            {
                _record = score;
            }
        }
    }
}
