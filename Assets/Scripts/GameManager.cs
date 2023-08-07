using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // INSTANCE
    public static GameManager Instance;

    [Header("LEVELS")] [SerializeField] private List<GameObject> _levels = new List<GameObject>();
    private int _levelNo = 1;

    [Header("GUN")] [SerializeField] private Animator _gunAnimator;
    public Animator GunAnimator => _gunAnimator;

    [Header("BLOCK")] private List<GameObject> _blocks = new List<GameObject>();

    public List<GameObject> Blocks
    {
        get => _blocks;
        set => _blocks = value;
    }

    [SerializeField] private int _minBlockCount;
    public int MinBlockCount => _minBlockCount;
    [SerializeField] private int _maxBlockCount;
    public int MaxBlockCount => _maxBlockCount;

    [Header("CAMERA")] [SerializeField] private CameraController _cameraControl;
    public CameraController CameraControl => _cameraControl;

    // GAME
    public static bool IsGameStarted = true;
    public static bool IsCompleted;
    public static bool IsFailed;

    // SCORE
    public int Score
    {
        get => PlayerPrefs.GetInt("Score", 0);
        set => PlayerPrefs.SetInt("Score", value);
    }
    public int BestScore
    {
        get => PlayerPrefs.GetInt("BestScore", 0);
        set => PlayerPrefs.SetInt("BestScore", value);
    }
    //public int BestScore => PlayerPrefs.GetInt("BestScore", 0);

    [Header("BALL")] [SerializeField] private GameObject _ballContainer;
    public GameObject BallContainer => _ballContainer;

    public int BallCount
    {
        get => PlayerPrefs.GetInt("BallCount", 5);
        set => PlayerPrefs.SetInt("BallCount", value);
    }

    [Header("SHOT")]
    
    private int _shotCount;
    public int ShotCount
    {
        get => _shotCount;
        set => _shotCount = value;
    }

    [SerializeField] private int _maxShotCount;
    public int MaxShotCount => _maxShotCount;

    [Header("BOOL")]
    private bool _IsShotOver;
    public bool IsShotOver
    {
        get => _IsShotOver;
        set => _IsShotOver = value;
    }
    
    [Header("SOUND")] 
    [SerializeField] private AudioSource _bounceSound;
    public AudioSource BounceSound => _bounceSound;

    // ANIMATOR CODE
    private static readonly int Failed = Animator.StringToHash("IsFailed");
    
    private void Awake()
    {
        Instance = this;

        IsGameStarted = true;
        IsFailed = false;
        IsCompleted = false;
        
        _levelNo = PlayerPrefs.GetInt("LevelID", 1);
        LoadLevel(_levelNo);
    }
    
    private void Update()
    {
        if (IsGameStarted && Blocks.Count < 1 && !IsCompleted)          // Blokların hepsi yokedildi ise
        {
            IsCompleted = true;
            CameraControl.RotateToFront();
            UpdateScore();
            Invoke(nameof(OnLevelCompleted), 2f);
        }
        if(IsShotOver)                 // Hamle hakkı bitti ise
            CheckFailStatus();
    }
    
    public void OnLevelCompleted()
    {
        if (IsGameStarted)
        {
            NextLevel();
            IsGameStarted = false;
        }
    }

    private void OnLevelFailed()
    {
        if (IsGameStarted)
        {
            UIManager.Instance.Invoke(nameof(UIManager.OpenFailPanel), 1f);
            _gunAnimator.SetBool(Failed, true);
            
            UpdateBestScore();
            IsGameStarted = false;
        }
    }
    
    private void LoadLevel(int levelCounter)
    {
        foreach (var level in _levels)
        {
            level.SetActive(false);
        }
        _levels[levelCounter - 1].SetActive(true);
        
        if(levelCounter == 1)
            Score = 0;
    }

    private void NextLevel()
    {
        if (_levelNo == _levels.Count) // son levelda ise
        {
            _levelNo = 1; // ilk level numarasi
            PlayerPrefs.SetInt("LevelID", _levelNo);
        }
        else
        {
            _levelNo++;
            PlayerPrefs.SetInt("LevelID", _levelNo);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        Score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Pause()
    {
        Time.timeScale = 0;
        
        UIManager.Instance.OpenPausePanel();
    }
    
    public void Resume()
    {
        Time.timeScale = 1;
        
        UIManager.Instance.ClosePausePanel();
    }

    private void UpdateScore()
    {
        if (IsFirstShot())
            Score += 5;
        else
            Score += 3;
        
        UIManager.Instance.SetScoreText(Score);
    }

    private bool IsFirstShot()
    {
        if(ShotCount == 1)
            return true;
        
        return false;
    }

    private void UpdateBestScore()
    {
        if (Score > BestScore)
        {
            BestScore = Score;
        }
        UIManager.Instance.SetBestScoreText(BestScore);
    }

    private void CheckFailStatus()
    {
        if (Blocks.Count > 0 && _ballContainer.transform.childCount == 0 )
        {
            OnLevelFailed();
        }
    }


}
