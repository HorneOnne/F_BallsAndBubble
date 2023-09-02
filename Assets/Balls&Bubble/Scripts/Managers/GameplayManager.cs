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
            STARTNEXTROUND,
            ROUNDFINISHED,
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
            ChangeGameState(GameState.PLAYING);
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
                case GameState.PLAYING:

                    OnPlaying?.Invoke();
                    break;
                case GameState.WAITING:


                    break;
                case GameState.ROUNDFINISHED:

                    OnRoundFinished?.Invoke();
                    ChangeGameState(GameState.STARTNEXTROUND);
                    break;
                case GameState.STARTNEXTROUND:

                    OnStartNextRound?.Invoke();
                    break;
                case GameState.WIN:


                    OnWin?.Invoke();
                    break;
                case GameState.GAMEOVER:
        
                    StartCoroutine(Utilities.WaitAfter(0.5f, () =>
                    {
                        SoundManager.Instance.PlaySound(SoundType.GameOver, false);
                        UIGameplayManager.Instance.CloseAll();
                        UIGameplayManager.Instance.DisplayGameoverMenu(true);
                    }));
                    OnGameOver?.Invoke();
                    break;
                case GameState.PAUSE:
                    Time.timeScale = 0.0f;
                    break;
                case GameState.UNPAUSE:
                    Time.timeScale = 1.0f;
                    _currentState = _gameStateWhenPause;
                    break;
                case GameState.EXIT:
                    Time.timeScale = 1.0f;
                    break;
            }
        }
    }
}

