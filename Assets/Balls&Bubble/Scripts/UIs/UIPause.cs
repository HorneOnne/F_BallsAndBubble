using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BallsAndBubble
{
    public class UIPause : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _homeBtn;
        [SerializeField] private Button _backBtn;




        private void Start()
        {
            _backBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.UNPAUSE);

                UIGameplayManager.Instance.CloseAll();
                UIGameplayManager.Instance.DisplayGameplayMenu(true);
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

    }
}
