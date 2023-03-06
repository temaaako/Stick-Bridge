using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Platforms")]
    [SerializeField] private float _startPlatformMinSize = 1f;
    [SerializeField] private float _startPlatformMaxSize = 3f;

    [SerializeField] private float _startPlatformMinDistance = 1f;
    [SerializeField] private float _startPlatformMaxDistance = 3f;

    private float _platformMinSize = 1f;
    private float _platformMaxSize = 3f;
    private float _platformMinDistance = 1f;
    private float _platformMaxDistance = 3f;


    private Difficulty _difficulty = Difficulty.EASY;

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

    private void OnGameOver(int obj)
    {
        _difficulty= Difficulty.EASY;
    }

    public PlatformGenerationSettings GetGenerationSettings()
    {
        return new PlatformGenerationSettings(_platformMinSize, _platformMaxSize, _platformMinDistance, _platformMaxDistance);
    }

    public PlatformGenerationSettings GetStartingGenerationSettings()
    {
        return new PlatformGenerationSettings(_startPlatformMinSize, _platformMaxSize, _startPlatformMinDistance, _startPlatformMaxDistance);
    }

    private void OnScoreChanged(int score)
    {
        switch (score)
        {
            case 10:
                _difficulty = Difficulty.MEDIUM;
                break;
            case 20:
                _difficulty = Difficulty.HARD; 
                break;

            default:
                break;
        }
    }
}
