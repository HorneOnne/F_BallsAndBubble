using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BallsAndBubble
{
    public class UIGameover : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _homeBtn;
        [SerializeField] private Button _backBtn;


        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _scoreValueText;

        private void OnEnable()
        {
            GameplayManager.OnGameOver += LoadScore;
        }

        private void OnDisable()
        {
            GameplayManager.OnGameOver -= LoadScore;
        }

        private void Start()
        {
            LoadScore();

            _backBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.EXIT);
                Loader.Load(Loader.Scene.GameplayScene);
            });

            _homeBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.EXIT);
                Loader.Load(Loader.Scene.MenuScene);
            });
        }

        private void OnDestroy()
        {
            _backBtn.onClick.RemoveAllListeners();
            _homeBtn.onClick.RemoveAllListeners();
        }

        private void LoadScore()
        {
            _scoreValueText.text = $"{0}";
        }

    }
}
