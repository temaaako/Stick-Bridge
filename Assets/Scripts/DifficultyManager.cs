using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Platforms")]
    


    [SerializeField] private Difficulty[] _difficulties;

    [SerializeField] private int _difficultyStep = 5;

    private Difficulty _difficulty;

    private int _difficultyNum=0;

    private int _lastChangeNum=0;

    private void OnEnable()
    {
        EventManager.Instance.scoreChanged += OnScoreChanged;
        EventManager.Instance.gameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.Instance.scoreChanged -= OnScoreChanged;
        EventManager.Instance.gameOver -= OnGameOver;
    }


    private void Awake()
    {
        SetStartingDifficulty();
    }

    private void OnGameOver(int obj)
    {
        SetStartingDifficulty();
    }

    public PlatformGenerationSettings GetGenerationSettings()
    {
        return _difficulty.GetGenerationSettings();
    }

    public PlatformGenerationSettings GetStartingGenerationSettings()
    {
        
        return _difficulties[0].GetGenerationSettings();
    }

    private void OnScoreChanged(int score)
    {
        if (_difficulties.Length - 1 <= _difficultyNum) return;


        Debug.Log($"_lastChangeNum: {_lastChangeNum}, difficultyStep {_difficultyStep}, score {score}");

        if (_lastChangeNum+_difficultyStep<=score)
        {
            _lastChangeNum= score;
            _difficultyNum++;
            _difficulty = _difficulties[_difficultyNum];

            Debug.Log(_difficulty.name);
        }
        
    }

    private void SetStartingDifficulty()
    {

        _difficultyNum = 0;

        _difficulty = _difficulties[_difficultyNum];

        _lastChangeNum = 0;
    }


    

   
}
