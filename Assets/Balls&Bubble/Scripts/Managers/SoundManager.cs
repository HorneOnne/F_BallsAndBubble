using UnityEngine;
using System.Collections.Generic;

namespace BallsAndBubble
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private static Dictionary<SoundType, float> _soundTimerDictionary;
        public SoundAudioClip[] SoundAudioClips;
        private GameObject _oneShotGameObject;
        private AudioSource _oneShotAudioSource;
        [Range(0, 1)]
        public float SFXVolume = 1.0f;

        [Header("Background")]
        public AudioClip BackgroundSoundClip;
        private AudioSource _backgroundAudioSource;
        [Range(0, 1)]
        public float BackgroundVolume = 1.0f;

        // Mute sound
        [HideInInspector] public bool isMusicActive = true;
        [HideInInspector] public bool isSoundFXActive = true;

        private void Awake()
        {
            // Check if an instance already exists, and destroy the duplicate
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;

            }
            Instance = this;
            Initialized();
            PlayBackgroundSound();
        }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            CreateOneShootAutdioSource();
        }



        public void Initialized()
        {
            _soundTimerDictionary = new Dictionary<SoundType, float>();
            _soundTimerDictionary[SoundType.HitBlock] = 0.0f;
            _soundTimerDictionary[SoundType.HitWall] = 0.0f;
            _soundTimerDictionary[SoundType.HitKey] = 0.0f;
            _soundTimerDictionary[SoundType.GameOver] = 0.0f;
            _soundTimerDictionary[SoundType.Win] = 0.0f;
            _soundTimerDictionary[SoundType.Button] = 0.0f;
            _soundTimerDictionary[SoundType.ScoreUp] = 0.0f;
            _soundTimerDictionary[SoundType.Shoot] = 0.0f;
        }


        public void PlaySound(SoundType soundType, bool playRandom, float pitch = 1.0f)
        {
            if (CanPlaySound(soundType) == false) return;
            if (_oneShotGameObject == null)
            {
                _oneShotGameObject = new GameObject("Sound");
                _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
                _oneShotAudioSource.volume = SFXVolume;
                _oneShotAudioSource.pitch = pitch;
                DontDestroyOnLoad(_oneShotAudioSource.gameObject);
            }
            else
            {
                _oneShotAudioSource = _oneShotGameObject.GetComponent<AudioSource>();
                _oneShotAudioSource.volume = SFXVolume;
                _oneShotAudioSource.pitch = pitch;
            }

            if (GetRandomAudioClip(soundType) != null)
            {
                if (playRandom)
                {
                    _oneShotAudioSource.PlayOneShot(GetRandomAudioClip(soundType));
                }
                else
                {
                    _oneShotAudioSource.PlayOneShot(GetFirstAudioClip(soundType));
                }
            }
        }

        public void PlaySound(SoundType soundType, bool playRandom, Vector2 position)
        {
            if (CanPlaySound(soundType) == false) return;
            if (_oneShotGameObject == null)
            {
                _oneShotGameObject = new GameObject("Sound");
                _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
            }

            if (playRandom)
            {
                _oneShotAudioSource.clip = GetRandomAudioClip(soundType);
            }
            else
            {
                _oneShotAudioSource.clip = GetFirstAudioClip(soundType);
            }

            _oneShotAudioSource.Play();
        }

        public void PlaySound(SoundType soundType, AudioClip audioClip)
        {
            if (CanPlaySound(soundType) == false) return;
            if (_oneShotGameObject == null)
            {
                _oneShotGameObject = new GameObject("Sound");
                _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
            }
            _oneShotAudioSource.PlayOneShot(audioClip);
        }


        private AudioClip GetFirstAudioClip(SoundType soundType)
        {
            foreach (var soundAudioClip in SoundAudioClips)
            {
                if (soundAudioClip.soundType.Equals(soundType))
                {
                    if (soundAudioClip.audioClips.Count > 0)
                        return soundAudioClip.audioClips[0];
                    else
                        return null;
                }
            }

            Debug.LogError($"Sound {soundType} not found!");
            return null;
        }

        private AudioClip GetRandomAudioClip(SoundType soundType)
        {
            foreach (var soundAudioClip in SoundAudioClips)
            {
                if (soundAudioClip.soundType.Equals(soundType))
                {
                    return soundAudioClip.audioClips[Random.Range(0, soundAudioClip.audioClips.Count)];
                }
            }

            Debug.LogError($"Sound {soundType} not found!");
            return null;
        }


        private bool CanPlaySound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.HitBlock:
                    return CanSoundTypePlay(soundType, 0.01f);
                case SoundType.HitKey:
                    return CanSoundTypePlay(soundType, 0.01f);
                case SoundType.HitWall:
                    return CanSoundTypePlay(soundType, 0.01f);
                case SoundType.Button:
                    return CanSoundTypePlay(soundType, 0.05f);
                case SoundType.ScoreUp:
                    return CanSoundTypePlay(soundType, 0.01f);
                case SoundType.GameOver:
                    return CanSoundTypePlay(soundType, 0.01f);
                case SoundType.Win:
                    return CanSoundTypePlay(soundType, 0.01f);
                case SoundType.Shoot:
                    return CanSoundTypePlay(soundType, 0.01f);
                default:
                    return true;
            }
        }

        private bool CanSoundTypePlay(SoundType soundType, float maxTimePlay)
        {
            if (_soundTimerDictionary.ContainsKey(soundType))
            {
                float lastTimePlayed = _soundTimerDictionary[soundType];
                if (lastTimePlayed + maxTimePlay < Time.time)
                {
                    _soundTimerDictionary[soundType] = Time.time;
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        private void CreateOneShootAutdioSource()
        {
            if (_oneShotGameObject == null)
            {
                _oneShotGameObject = new GameObject("Sound");
                _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
                _oneShotAudioSource.volume = SFXVolume;
                _oneShotAudioSource.pitch = 1.0f;
                DontDestroyOnLoad(_oneShotAudioSource.gameObject);
            }
        }

        public void MuteSoundFX(bool mute)
        {
            _oneShotAudioSource.mute = mute;
        }

        public void MuteBackground(bool mute)
        {
            _backgroundAudioSource.mute = mute;
        }


        #region Background
        private void PlayBackgroundSound()
        {
            _backgroundAudioSource = this.gameObject.AddComponent<AudioSource>();
            _backgroundAudioSource.clip = BackgroundSoundClip;
            _backgroundAudioSource.volume = BackgroundVolume;
            _backgroundAudioSource.loop = true;
            _backgroundAudioSource.Play();
        }
        public void UpdateBackgroundVolume()
        {
            _backgroundAudioSource.volume = BackgroundVolume;
        }
        #endregion


        #region BallsAndBubble Addition
   
        public void AdjustVolume(float value = 0.25f)
        {
            SFXVolume += value;
            BackgroundVolume += value;

            if(SFXVolume < 0)
                SFXVolume = 0;
            if(SFXVolume > 1.0f)
                SFXVolume = 1.0f;

            if (BackgroundVolume < 0)
                BackgroundVolume = 0;
            if (BackgroundVolume > 1.0f)
                BackgroundVolume = 1.0f;

            UpdateBackgroundVolume();
        }
        #endregion
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundType soundType;
        public List<AudioClip> audioClips;
    }
    public enum SoundType
    {
        Wait,
        HitBlock,
        HitWall,
        HitKey,
        ScoreUp,
        GameOver,
        Win,
        Button,
        Shoot,
    }
}
