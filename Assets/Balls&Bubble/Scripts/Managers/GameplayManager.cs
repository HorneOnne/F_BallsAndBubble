using UnityEngine;


namespace BallsAndBubble
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance { get; private set; }
        public static event System.Action OnStateChanged;
        public static event System.Action OnPlaying;
        public static event System.Action OnWin;
        public static event System.Action OnGameOver;
        public static event System.Action OnRoundFinished;
        public static event System.Action OnStartNextRound;

        public enum GameState
        {
            PLAYING,
            WAITING,
            WIN,
            GAMEOVER,
            PAUSE,
            UNPAUSE,
            EXIT,
        }


        [Header("Properties")]
        [SerializeField] private GameState _currentState;
        private GameState _gameStateWhenPause;


        #region Properties
        public GameState CurrentState { get => _currentState; }
        #endregion


        #region Init & Events
        private void Awake()
        {
            Instance = this;

        }

        private void OnEnable()
        {
            OnStateChanged += SwitchState;
        }

        private void OnDisable()
        {
            OnStateChanged -= SwitchState;
        }

        private void Start()
        {
            Time.timeScale = 1.0f;
            GameManager.Instance.ResetScore();
        }
        #endregion



        public void ChangeGameState(GameState state)
        {
            _currentState = state;
            OnStateChanged?.Invoke();
        }

        public void CacheGameStateWhenPause(GameState state)
        {
            _gameStateWhenPause = state;
        }

        private void SwitchState()
        {
            switch (_currentState)
            {
                default: break;
                case GameState.WAITING:
                    

                    break;
                case GameState.PLAYING:
                    Time.timeScale = 1.0f;

                    OnPlaying?.Invoke();
                    break;            
                case GameState.WIN:

                    OnWin?.Invoke();
                    break;
                case GameState.GAMEOVER:
        
                    StartCoroutine(Utilities.WaitAfter(0.5f, () =>
                    {
                        SoundManager.Instance.PlaySound(SoundType.GameOver, false);
                        UIGameplayManager.Instance.DisplayGameoverMenu(true);
                    }));
                    OnGameOver?.Invoke();
                    break;
                case GameState.PAUSE:
                    Time.timeScale = 0.0f;
                    break;
                case GameState.UNPAUSE:
                    Time.timeScale = 1.0f;
                    _currentState = GameState.PLAYING;
                    break;
                case GameState.EXIT:
                    Time.timeScale = 1.0f;
                    break;
            }
        }
    }
}

