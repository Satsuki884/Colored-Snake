using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Audio Clips Music")]
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;

    [Header("Audio Clips SFX")]
    [SerializeField] private AudioClip _bombMusic;
    [SerializeField] private AudioClip _wallMusic;
    [SerializeField] private AudioClip _shieldMusic;
    [SerializeField] private AudioClip _eatMusic;
    [SerializeField] private AudioClip _speedMusic;

    private string _menu = "Menu";
    private string _game = "Game";

    public string Bomb = "bomb";
    public string Wall = "wall";
    public string Shield = "shield";
    public string Eat = "eat";
    public string Speed = "speed";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        SetMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetMusicForScene(scene.name);
    }

    private void SetMusicForScene(string sceneName)
    {
        if (_musicSource == null) return;

        AudioClip targetMusic = sceneName == _menu ? _menuMusic : _gameMusic;

        if (_musicSource.clip != targetMusic)
        {
            _musicSource.clip = targetMusic;
            _musicSource.Play();
        }
    }

    public void PlaySFX(string clipName)
    {
        switch (clipName)
        {
            case "bomb":
                _sfxSource.PlayOneShot(_bombMusic);
                break;
            case "wall":
                _sfxSource.PlayOneShot(_wallMusic);
                break;
            case "shield":
                _sfxSource.PlayOneShot(_shieldMusic);
                break;
            case "eat":
                _sfxSource.PlayOneShot(_eatMusic);
                break;
            case "speed":
                _sfxSource.PlayOneShot(_speedMusic);
                break;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
