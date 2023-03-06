using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text _highscoreText;

    [SerializeField] private TMP_Text _scoreText;
    
    [SerializeField] private Canvas _gameOverCanvas;


    private int _highscore;
    private int Highscore
    {
        get
        {
            return _highscore;
        }
        set
        {
            _highscore = value;
            PlayerPrefs.SetInt("Highscore", value);
            _highscoreText.text = value.ToString();
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.scoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
       _scoreText.text = score.ToString();
    }


    // Start is called before the first frame update
    void Awake()
    {
        _gameOverCanvas.enabled = false;
        EventManager.Instance.gameOver += OnGameOver;
        EventManager.Instance.gameRestarted += OnGameReset;
        Highscore = PlayerPrefs.GetInt("Highscore", 0);
    }

    private void OnGameOver(int userScore)
    {
        Debug.Log("GameOVer");
        _gameOverCanvas.enabled = true;
        if (userScore > Highscore)
        {
            Highscore = userScore;
        }
    }


    private void OnGameReset()
    {
        _gameOverCanvas.enabled = false;
    }
}
