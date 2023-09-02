using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BallsAndBubble
{
    public class UIPause : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _continueBtn;
        [SerializeField] private Button _backBtn;
        [SerializeField] private Button _musicBtn;
        [SerializeField] private Button _soundBtn;
        [SerializeField] private Button _homeBtn;


        [Header("Images")]
        [SerializeField] private Image _musicBtnIcon;
        [SerializeField] private Image _soundBtnIcon;



        private void Start()
        {
            UpdateMusicUI();
            UpdateSoundFXUI();

            _continueBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.UNPAUSE);

                UIGameplayManager.Instance.CloseAll();
                UIGameplayManager.Instance.DisplayGameplayMenu(true);
            });

            _backBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.EXIT);
                Loader.Load(Loader.Scene.MenuScene);
            });

            _musicBtn.onClick.AddListener(() =>
            {
                ToggleMusic();
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });

            _soundBtn.onClick.AddListener(() =>
            {
                ToggleSFX();
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });

            _homeBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);

                Loader.Load(Loader.Scene.MenuScene);
            });
        }

        private void OnDestroy()
        {
            _continueBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.RemoveAllListeners();
            _musicBtn.onClick.RemoveAllListeners();
            _soundBtn.onClick.RemoveAllListeners();
            _homeBtn.onClick.RemoveAllListeners();
        }


        private void ToggleSFX()
        {

            SoundManager.Instance.MuteSoundFX(SoundManager.Instance.isSoundFXActive);
            SoundManager.Instance.isSoundFXActive = !SoundManager.Instance.isSoundFXActive;

            UpdateSoundFXUI();
        }


        private void UpdateSoundFXUI()
        {
            if (SoundManager.Instance.isSoundFXActive)
            {
                SetImageOpacity(_soundBtnIcon, 1.0f);
                SetImageOpacity(_soundBtn.image, 1.0f);
            }
            else
            {
                SetImageOpacity(_soundBtnIcon, 0.4f);
                SetImageOpacity(_soundBtn.image, 0.4f);
            }
        }

        private void ToggleMusic()
        {
            SoundManager.Instance.MuteBackground(SoundManager.Instance.isMusicActive);
            SoundManager.Instance.isMusicActive = !SoundManager.Instance.isMusicActive;

            UpdateMusicUI();
        }

        private void UpdateMusicUI()
        {
            if (SoundManager.Instance.isMusicActive)
            {
                //_musicBtn.image.sprite = _unmuteBtnSprite;
                SetImageOpacity(_musicBtnIcon, 1.0f);
                SetImageOpacity(_musicBtn.image, 1.0f);
            }
            else
            {
                //_musicBtn.image.sprite = _muteBtnSprite;
                SetImageOpacity(_musicBtnIcon, 0.4f);
                SetImageOpacity(_musicBtn.image, 0.4f);
            }
        }

        public void SetImageOpacity(Image image, float alpha)
        {
            Color currentImageColor = image.color;
            currentImageColor.a = alpha;
            image.color = currentImageColor;
        }

    }
}
