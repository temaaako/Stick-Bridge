using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.Audio;

public class Game : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource _scalingSound;
    [SerializeField] private AudioSource _dropSound;
    [SerializeField] private float _startingPitch = 1f;
    [SerializeField] private float _stepPitch = 0.5f;





    [Header("Other")]

    [SerializeField] private Player _player;

    [SerializeField] private Stick _stickPrefab;
    [SerializeField] private float _stickOffset;
    [SerializeField] private float _stickScaleSpeed;

    [SerializeField] private float _platformFallTime;

    [SerializeField] private Platform _currentPlatform;

    [HideInInspector] public Platform CurrentPlatform => _currentPlatform;

    [SerializeField] private Platform _nextPlatform;

    [SerializeField] private TMP_Text _scoreText;


    [SerializeField] private Vector2 _firstPlatformPosition = new Vector2(-2, -2);
    [SerializeField] private float _firstPlatformSize = 2.48f;

    private bool IsInputAllowed = true;


  


    private int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            _scoreText.text = value.ToString();
        }
    }


    private PlatformGenerator platformGenerator;

    [SerializeField] private float _allPlatformMoveDuration = 2;
    private AllPlatforms allPlatforms;

    private Vector3 _stickPos;
    private Stick _stick;

    private Action stickFall;
    private Action stepEnd;
    public Action<int> gameOver;
    public Action gameReset;

    private float stickWidth;

    
    public int FrameRate =60;

    private void Start()
    {
        Application.targetFrameRate = FrameRate;


        QualitySettings.vSyncCount = 0;
        platformGenerator = FindObjectOfType<PlatformGenerator>();
        allPlatforms = FindObjectOfType<AllPlatforms>();

        stickWidth = _stickPrefab.transform.GetChild(0).transform.localScale.x;

        SetStartingPlatforms();

    }


    private void SetStartingPlatforms()
    {
        _currentPlatform = platformGenerator.Generate(_firstPlatformPosition, _firstPlatformSize);
        _nextPlatform = platformGenerator.Generate(_currentPlatform.GetRightEdgePosition(), false);
    }


    private void Awake()
    {
        stickFall += OnStickFall;
        stepEnd += OnStepEnd;
        gameOver += OnGameOver;
        gameReset += OnGameReset;
    }


    void Update()
    {

        if (IsInputAllowed == false) return;

        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if (_stick == null)
            {
                Debug.Log("creating stick");
                createStick();
            }
            else
            {
                scaleStick();


                _scalingSound.pitch +=_stepPitch;

                if (_scalingSound.isPlaying==false)
                {
                    _scalingSound.Play();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            if (_stick == null) return;
            StartCoroutine(DoRotation());
            _scalingSound.Stop();
            _scalingSound.pitch = _startingPitch;

        }


    }
    private IEnumerator DoRotation()
    {
        IsInputAllowed = false;
        _stick.transform.DORotate(new Vector3(0, 0, -90), _platformFallTime).SetUpdate(UpdateType.Normal, true);
        yield return new WaitForSeconds(_platformFallTime);
        stickFall?.Invoke();
    }


    Vector3 GetNewCameraPosition()
    {
        return _currentPlatform.transform.position + new Vector3(_currentPlatform.GetComponent<SpriteRenderer>().size.x / 2 - _stickOffset - stickWidth / 2, 0, 0) - _player.transform.position;
    }



    private void scaleStick()
    {
        _stick.transform.localScale += new Vector3(0, _stickScaleSpeed*Time.deltaTime);
    }

    private void createStick()
    {
        _stickPos = _player.transform.position + new Vector3(_stickOffset, -_player.transform.localScale.y / 2, 0);
        _stick = Instantiate(_stickPrefab, _stickPos, Quaternion.identity, allPlatforms.transform);
    }




    private IEnumerator MakeTransition()
    {
        allPlatforms.transform.DOMove(allPlatforms.transform.position - new Vector3(GetNewCameraPosition().x, 0, 0), _allPlatformMoveDuration);
        _player.Run();
        yield return new WaitForSeconds(_allPlatformMoveDuration);
        _player.Idle();
        Destroy(_stick.gameObject);
        Debug.Log(_stick);
        IsInputAllowed = true;

    }

    public void DestroyAllPlatforms()
    {
        var objects = GameObject.FindObjectsOfType<Platform>();
        foreach (Platform o in objects)
        {
            Destroy(o.gameObject);
        }
    }

    public void ResetGame()
    {
        gameReset?.Invoke();
    }




    private void OnGameReset()
    {

        if (_stick != null)
        {
            Debug.Log("stick!=null");
            Destroy(_stick.gameObject);

        }

        Score = 0;
        DestroyAllPlatforms();
        SetStartingPlatforms();
        _stick = null;

        IsInputAllowed = true;


    }

    private void OnStickFall()
    {
        float nextPlatformLeftLength = _nextPlatform.transform.position.x - _nextPlatform.GetComponent<SpriteRenderer>().size.x / 2 - _stick.transform.position.x;
        float nextPlatformRightLength = nextPlatformLeftLength + _nextPlatform.GetComponent<SpriteRenderer>().size.x;

        Debug.Log(nextPlatformLeftLength + " " + nextPlatformRightLength);
        if (nextPlatformLeftLength <= _stick.transform.localScale.y && _stick.transform.localScale.y <= nextPlatformRightLength)
        {
            Debug.Log("Нормально");
            stepEnd?.Invoke();

        }
        else
        {
            Debug.Log("Game over");
            gameOver?.Invoke(Score);
        }
    }

    private void OnStepEnd()
    {
        _dropSound.Play();

        _currentPlatform = _nextPlatform;
        _nextPlatform = platformGenerator.Generate(_currentPlatform.GetRightEdgePosition(), true);
        Score++;
        StartCoroutine(MakeTransition());
    }

    private void OnGameOver(int score)
    {

    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_player.transform.position + new Vector3(_stickOffset, -_player.transform.localScale.y / 2, 0), 0.05f);


    }
}
