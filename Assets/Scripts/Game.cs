using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class Game : MonoBehaviour
{

    [SerializeField] public Player _player;
    [SerializeField] private Platform _currentPlatform;
    [SerializeField] public Platform _nextPlatform;
    [SerializeField] private float _allPlatformMoveDuration = 2;

    private PlatformGenerator platformGenerator;
    private AllPlatforms allPlatforms;
    private StickManager _stickManager;
    private BridgeChecker _bridgeChecker;
    private DifficultyManager _difficultyManager;
    private int _score;


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
        allPlatforms = FindObjectOfType<AllPlatforms>();
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
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            EventManager.Instance.InputEnded();
        }
    }
    
    Vector3 GetNewCameraPosition()
    {
        return _currentPlatform.transform.position + new Vector3(_currentPlatform.GetComponent<SpriteRenderer>().size.x / 2 - _stickManager.StickOffset - _stickManager.Stick.Width / 2, 0, 0) - _player.transform.position;
    }

    private IEnumerator MakeTransition()
    {
        allPlatforms.transform.DOMove(allPlatforms.transform.position - new Vector3(GetNewCameraPosition().x, 0, 0), _allPlatformMoveDuration);
        _player.Run();
        yield return new WaitForSeconds(_allPlatformMoveDuration);
        _player.Idle();
        _stickManager.Stick.gameObject.SetActive(false);
        EventManager.Instance.TransitionEnded();
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
        if (_bridgeChecker.BridgeWorks(_stickManager.Stick, _nextPlatform))
        {
            _currentPlatform = _nextPlatform;
            _nextPlatform = platformGenerator.Generate(_currentPlatform.GetRightEdgePosition(), _difficultyManager.GetGenerationSettings(), true);
            Score++;
            StartCoroutine(MakeTransition());
        }
        else
        {
            EventManager.Instance.GameOver(Score);
        }
    }


    public void SetNextPlatform(Platform platform)
    {
        _nextPlatform = platform;
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(_player.transform.position + new Vector3(_stickManager.stickOffset, -_player.transform.localScale.y / 2, 0), 0.05f);
    //}
}
