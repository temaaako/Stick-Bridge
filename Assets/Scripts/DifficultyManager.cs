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
        _difficulty = Difficulty.EASY;
    }

    public PlatformGenerationSettings GetGenerationSettings()
    {
        return GetStartingGenerationSettings();
        //return new PlatformGenerationSettings(_platformMinSize, _platformMaxSize, _platformMinDistance, _platformMaxDistance);
    }

    public PlatformGenerationSettings GetStartingGenerationSettings()
    {
        return new PlatformGenerationSettings(_startPlatformMinSize, _startPlatformMaxSize, _startPlatformMinDistance, _startPlatformMaxDistance);
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


    [SerializeField] private GameObject startPointObject;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rightEdge = startPointObject.transform.position + new Vector3(startPointObject.GetComponent<SpriteRenderer>().size.x / 2, startPointObject.GetComponent<SpriteRenderer>().size.y / 2, 0);  
        Gizmos.DrawSphere(rightEdge, 0.05f);
        Gizmos.DrawLine(rightEdge + new Vector3(_startPlatformMinDistance, 0, 0), rightEdge + new Vector3(_startPlatformMaxDistance + _startPlatformMaxSize, 0, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(rightEdge + new Vector3(_startPlatformMinDistance, 0.2f, 0), rightEdge + new Vector3(_startPlatformMinDistance + _startPlatformMaxSize, 0.2f, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rightEdge + new Vector3(_startPlatformMinDistance, 0.4f, 0), rightEdge + new Vector3(_startPlatformMinDistance + _startPlatformMinSize  , 0.4f, 0));
    }
}
