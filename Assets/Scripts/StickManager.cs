using DG.Tweening;
using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StickManager : MonoBehaviour
{

    [SerializeField] private Stick _stickPrefab;
    [SerializeField] private float _stickOffset;
    public float StickOffset => _stickOffset;
    [SerializeField] private float _stickScaleSpeed;
    [SerializeField] private float _stickFallTime;
    [SerializeField] private Stick _stick;
    public Stick Stick => _stick;

    private Vector3 _stickPos;

    private Game _game;

    private Player _player;

    [SerializeField] private GameObject _allPlatforms;


    public bool StickIsNull => _stick == null;

    void Awake()
    {
        _player = FindObjectOfType<Player>();
        _game = FindObjectOfType<Game>();
        if (_stick.gameObject.activeSelf == true)
        {
            _stick.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.inputIsHolding += OnInputIsHeld;
        EventManager.Instance.inputEnded += OnInputEnded;
        EventManager.Instance.gameRestarted += OnGameRestarted;
        EventManager.Instance.transitionEnded += OnTransitionEnded;
    }

    private void OnDisable()
    {
        EventManager.Instance.inputIsHolding -= OnInputIsHeld;
        EventManager.Instance.inputEnded -= OnInputEnded;
        EventManager.Instance.gameRestarted -= OnGameRestarted;
        EventManager.Instance.transitionEnded -= OnTransitionEnded;
    }

    private void OnTransitionEnded()
    {
        _stick.gameObject.SetActive(false);

        _stick.SetStartingSize();
    }

    private void OnInputEnded()
    {

        if (_stick == null) return;
        StartCoroutine(DoRotation());
    }
    private IEnumerator DoRotation()
    {
        _game.IsInputAllowed = false;
        _stick.transform.DORotate(new Vector3(0, 0, -90), _stickFallTime).SetUpdate(UpdateType.Normal, true);
        yield return new WaitForSeconds(_stickFallTime);
        EventManager.Instance.StickFell();
    }

    private void OnInputIsHeld()
    {
        if (_stick.gameObject.activeSelf == false)
        {
            Debug.Log("activateStick");
            ActivateStick();
        }
        else
        {
            ScaleStick();
        }
    }

    private void ActivateStick()
    {
        _stickPos = _player.transform.position + new Vector3(_stickOffset, -_player.transform.localScale.y / 2, 0);
        _stick.transform.position = _stickPos;
        _stick.transform.rotation = new Quaternion(0, 0, 0, 0);
        _stick.gameObject.SetActive(true);
    }

    private void ScaleStick()
    {
        _stick.transform.localScale += new Vector3(0, _stickScaleSpeed * Time.deltaTime);
    }

    private void OnGameRestarted()
    {
        _stick.gameObject.SetActive(false);
        _stick.SetStartingSize();
        StartCoroutine(EnableInputAfterDelay());
    }

    private IEnumerator EnableInputAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        _game.IsInputAllowed = true;
    }

 
}
