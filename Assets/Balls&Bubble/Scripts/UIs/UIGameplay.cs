using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BallsAndBubble
{
    public class UIGameplay : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _pauseBtn;
        [SerializeField] private Button _replayBtn;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _scoreText;



        private void OnEnable()
        {
            GameManager.OnScoreUp += LoadScoreText;
        }

        private void OnDisable()
        {
            GameManager.OnScoreUp -= LoadScoreText;
        }


        private void Start()
        {
            LoadScoreText();

            _pauseBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.PAUSE);

                UIGameplayManager.Instance.DisplayPauseMenu(true);
            });


            _replayBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                Loader.Load(Loader.Scene.GameplayScene);
            });

        }

        private void OnDestroy()
        {
            _pauseBtn.onClick.RemoveAllListeners();
        }



        private void LoadScoreText()
        {
            _scoreText.text = $"{GameManager.Instance.Score}";
        }
    }
}
