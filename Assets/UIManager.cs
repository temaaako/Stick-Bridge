using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text _highscoreText;

    [SerializeField] private Canvas _gameOverCanvas;
    Game game;


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

    // Start is called before the first frame update
    void Start()
    {
        _gameOverCanvas.enabled = false;
        game = FindObjectOfType<Game>();
        game.gameOver += OnGameOver;
        game.gameReset += OnGameReset;
        Highscore = PlayerPrefs.GetInt("Highscore", 0);
    }

    private void OnGameOver(int userScore)
    {
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
