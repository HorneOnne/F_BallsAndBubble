using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace BallsAndBubble
{
    public class UIMainMenu : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _classicBtn;
        [SerializeField] private Button _survivalBtn;
        [SerializeField] private Button _musicBtn;
        [SerializeField] private Button _soundBtn;
        [SerializeField] private Button _quitBtn;


        [Header("Images")]
        [SerializeField] private Image _musicBtnIcon;
        [SerializeField] private Image _soundBtnIcon;



        private void Start()
        {
            UpdateMusicUI();
            UpdateSoundFXUI();

            _classicBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameManager.Instance.PlayType = GameManager.GamePlayType.CLASSIC;
                Loader.Load(Loader.Scene.GameplayScene);
            });

            _survivalBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameManager.Instance.PlayType = GameManager.GamePlayType.SURVIVAL;
                Loader.Load(Loader.Scene.GameplayScene);
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

            _quitBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                Application.Quit();

                // For the Unity Editor, this will not quit the application. It will stop the editor's play mode.
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            });
        }

        private void OnDestroy()
        {
            _classicBtn.onClick.RemoveAllListeners();
            _survivalBtn.onClick.RemoveAllListeners();
            _musicBtn.onClick.RemoveAllListeners();
            _soundBtn.onClick.RemoveAllListeners();
            _quitBtn.onClick.RemoveAllListeners();
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
