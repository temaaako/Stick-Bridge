using UnityEngine;
using System;

public class Game : MonoBehaviour
{

    [SerializeField] public Platform currentPlatform;
    [SerializeField] public Platform nextPlatform;
    [SerializeField] private float _maxInputTime = 10f;

    private PlatformGenerator platformGenerator;
    private StickManager _stickManager;
    private BridgeChecker _bridgeChecker;
    private DifficultyManager _difficultyManager;
    private int _score;
    private float _inputTime = 0f;

    public bool IsInputAllowed = true;


    public int FrameRate =60;
    public int Score
    {
        get { return _score; }
        set
        {
            if (value>=0)
            {
                EventManager.Instance.ScoreChanged(value);
            }
            else
            {
                throw new ArgumentException("Value cannot be less than 0", nameof(value));
            }
        }
    }
  
    private void Awake()
    {
        Application.targetFrameRate = FrameRate;
        QualitySettings.vSyncCount = 0;
        platformGenerator = FindObjectOfType<PlatformGenerator>();
        _stickManager= FindObjectOfType<StickManager>();
        _bridgeChecker = new BridgeChecker();
        _difficultyManager = FindObjectOfType<DifficultyManager>();
    }
    
    private void OnEnable()
    {
        EventManager.Instance.stickFell += OnStickFell;
        EventManager.Instance.gameRestarted += OnGameRestarted;
        EventManager.Instance.scoreChanged += OnScoreChanged;
        EventManager.Instance.transitionEnded += OnTransitionEnded;
    }

    private void OnDisable()
    {
        EventManager.Instance.stickFell -= OnStickFell;
        EventManager.Instance.gameRestarted -= OnGameRestarted;
        EventManager.Instance.scoreChanged -= OnScoreChanged;
        EventManager.Instance.transitionEnded -= OnTransitionEnded;
    }

    private void OnGameRestarted()
    {
        Score = 0;
    }

    private void OnScoreChanged(int score)
    {
        _score = score;
    }

    void Update()
    {
        if (IsInputAllowed == false) return;

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            EventManager.Instance.InputIsHolding();
            _inputTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)||_inputTime>=_maxInputTime)
        {
            EventManager.Instance.InputEnded();
            _inputTime = 0f;
        }
    }
    

    private void OnTransitionEnded()
    {
        IsInputAllowed = true;
    }


    public void ResetGame()
    {
        EventManager.Instance.GameRestarted();
    }


    private void OnStickFell()
    {
        if (_bridgeChecker.BridgeWorks(_stickManager.Stick, nextPlatform))
        {
            EventManager.Instance.TransitionStarted(new Vector3 (currentPlatform.GetRightEdgeXPosition().x-nextPlatform.GetRightEdgeXPosition().x, 0 , 0));
            currentPlatform = nextPlatform;
            nextPlatform = platformGenerator.Generate(currentPlatform.GetRightEdgeXPosition(), _difficultyManager.GetGenerationSettings(), true);
            Score++;
            
        }
        else
        {
            EventManager.Instance.GameOver(Score);
        }
    }


    public void SetNextPlatform(Platform platform)
    {
        nextPlatform = platform;
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(_player.transform.position + new Vector3(_stickManager.stickOffset, -_player.transform.localScale.y / 2, 0), 0.05f);
    //}
}
