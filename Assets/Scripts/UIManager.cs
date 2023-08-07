using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    // INSTANCE
    public static UIManager Instance;

    [Header("SHOT COUNT")]
    [SerializeField] private TMP_Text _shotCountText;
    
    [Header("BALL COUNT")]
    [SerializeField] private TMP_Text _ballCountText;
    
    [Header("EXTRA BALL BAR")]
    [SerializeField] private RectTransform _extraBallBarFill;
    [SerializeField]private float _fillStartWidth;
    [SerializeField]private float _fillMaxWidth;
    private float _currentWidth;

    [Header("FAIL")] 
    [SerializeField] private GameObject _failPanel;
    
    [Header("SCORE")] 
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _bestScoreText;
    
    [Header("PAUSE")] 
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _pause;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        SetBallCountText(PlayerPrefs.GetInt("BallCount", 5).ToString());

        _extraBallBarFill.sizeDelta = new Vector2(_fillStartWidth, _extraBallBarFill.sizeDelta.y);
        _currentWidth = _fillStartWidth;

        SetScoreText(GameManager.Instance.Score);
        ResetUI();
    }

    private void ResetUI()
    {
        _pausePanel.SetActive(false);
        _failPanel.SetActive(false);
    }

    public void OpenFailPanel()
    {
        _failPanel.SetActive(true);
    }
    
    public void OpenPausePanel()
    {
        _pause.SetActive(false);
        _pausePanel.SetActive(true);
    }
    
    public void ClosePausePanel()
    {
        _pause.SetActive(true);
        _pausePanel.SetActive(false);
    }

    public void UpdateShotCountText()
    {
        int shotCount = GameManager.Instance.ShotCount;
        int maxShotCount = GameManager.Instance.MaxShotCount;
        
        if (shotCount == maxShotCount)
        {
            // hamle hakkı bitti
            _shotCountText.text = "FINAL";
        }
        else if(shotCount < maxShotCount)
        {
            _shotCountText.text = shotCount + "/" + maxShotCount;
        }
    }
    public void SetBallCountText(string text)
    {
        _ballCountText.text = text;
    }

    public void UpdateExtraBallBar()
    {
        _currentWidth = Random.Range(80, 120) + _fillStartWidth + _currentWidth % 576f;
        
        if (_currentWidth >= _fillMaxWidth)
        {
            GameManager.Instance.BallCount++;
            SetBallCountText(GameManager.Instance.BallCount.ToString());
            _currentWidth = _fillStartWidth;
        }
        _extraBallBarFill.sizeDelta = new Vector2(_currentWidth, _extraBallBarFill.sizeDelta.y);
    }

    public void SetScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }
    public void SetBestScoreText(int bestScore)
    {
        _bestScoreText.text = "BEST " + bestScore;
    }
    
}
