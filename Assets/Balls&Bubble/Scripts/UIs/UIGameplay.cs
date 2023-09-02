using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BallsAndBubble
{
    public class UIGameplay : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _pauseBtn;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _pauseBtnText;
        [SerializeField] private TextMeshProUGUI _scoreText;



        private void OnEnable()
        {
            GameplayManager.OnStartNextRound += LoadScoreText;
        }

        private void OnDisable()
        {
            GameplayManager.OnStartNextRound -= LoadScoreText;
        }


        private void Start()
        {
            LoadScoreText();

            _pauseBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.CacheGameStateWhenPause(GameplayManager.Instance.CurrentState);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.PAUSE);

                UIGameplayManager.Instance.CloseAll();
                UIGameplayManager.Instance.DisplayPauseMenu(true);
            });


        }

        private void OnDestroy()
        {
            _pauseBtn.onClick.RemoveAllListeners();
        }



        private void LoadScoreText()
        {
            _scoreText.text = $"{0}";
        }
    }
}
