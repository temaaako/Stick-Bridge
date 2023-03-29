using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{

    [SerializeField] private Platform prefab;

    

    [SerializeField] private float _moveUpDuration=0.2f;

    [SerializeField] private float _startAnimationY = -10f;

    [SerializeField] private Vector2 _firstPlatformPosition = new Vector2(-2, -2);
    [SerializeField] private float _firstPlatformSize = 2.48f;
    [SerializeField] private float _screenBorderXOffset=0.2f;

    

    private Game _game;
    private AllPlatforms _allPlatforms;
    private DifficultyManager _difficultyManager;

    private float _screenBorderXCoord;



    private void OnEnable()
    {
        EventManager.Instance.gameRestarted += OnGameReset;
    }

    private void OnDisable()
    {
        EventManager.Instance.gameRestarted -= OnGameReset;
    }

    private void Awake()
    {
         _allPlatforms=FindObjectOfType<AllPlatforms>();
        _game = FindObjectOfType<Game>();
        _difficultyManager = FindObjectOfType<DifficultyManager>();

        _screenBorderXCoord = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - _screenBorderXOffset;

        
        Debug.Log(_screenBorderXCoord);
    }


    private void Start()
    {
        SetStartingPlatforms();
    }

    public Platform Generate(Vector3 currentPlatformEdgePos, PlatformGenerationSettings generationSettings, bool isAnimated)
    {
        

        float sizeX = Random.Range(generationSettings.minSize, generationSettings.maxSize);

        float distance = Random.Range(generationSettings.minDistance, generationSettings.maxDistance);

        Vector3 position = new Vector3(currentPlatformEdgePos.x + distance + sizeX / 2, currentPlatformEdgePos.y, currentPlatformEdgePos.z);
        Platform platform;

        if (isAnimated)
        {
            Vector3 startPosition = new Vector3(currentPlatformEdgePos.x + distance + sizeX / 2, _startAnimationY, currentPlatformEdgePos.z);
            platform = Instantiate(prefab, startPosition, Quaternion.identity, _allPlatforms.transform);
            platform.transform.DOMoveY(position.y, _moveUpDuration).SetEase(Ease.InSine);
        }
        else
        {
            platform = Instantiate(prefab, position, Quaternion.identity, _allPlatforms.transform);
        }

        platform.GetComponent<SpriteRenderer>().size = new Vector3(sizeX, platform.GetComponent<SpriteRenderer>().size.y);

        return platform;
    }

    private Platform Generate(Vector3 position, float size)
    {

        Platform platform = Instantiate(prefab, position, Quaternion.identity, _allPlatforms.transform);
        platform.GetComponent<SpriteRenderer>().size = new Vector3(size, platform.GetComponent<SpriteRenderer>().size.y);

        return platform;
    }


    private void SetStartingPlatforms()
    {
        _game.currentPlatform = Generate(_firstPlatformPosition, _firstPlatformSize);
        _game.nextPlatform = Generate(_game.currentPlatform.GetRightEdgeXPosition(), _difficultyManager.GetStartingGenerationSettings(), false);    
    }

    private void DestroyAllPlatforms()
    {
        var objects = FindObjectsOfType<Platform>();
        foreach (Platform o in objects)
        {
            Destroy(o.gameObject);
        }
    }

    private void OnGameReset()
    {
        DestroyAllPlatforms();
        SetStartingPlatforms();
    }

}
