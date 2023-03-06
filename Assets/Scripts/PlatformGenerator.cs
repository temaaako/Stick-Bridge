using DG.Tweening;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    [SerializeField] private Platform prefab;

    

    [SerializeField] private float _moveUpDuration=0.2f;

    [SerializeField] private float _startAnimationY = -10f;


   


    [SerializeField] private Vector2 _firstPlatformPosition = new Vector2(-2, -2);
    [SerializeField] private float _firstPlatformSize = 2.48f;


    private Game _game;
    private AllPlatforms _allPlatforms;
    private DifficultyManager _difficultyManager;

    private void Awake()
    {
         _allPlatforms=FindObjectOfType<AllPlatforms>();
        _game = FindObjectOfType<Game>();
        _difficultyManager = FindObjectOfType<DifficultyManager>();
    }


    private void Start()
    {
        SetStartingPlatforms();
    }

    public Platform Generate(Vector3 edgePos, PlatformGenerationSettings generationSettings, bool isAnimated)
    {
        float sizeX = Random.Range(generationSettings.minSize, generationSettings.maxSize);
        float distance = Random.Range(generationSettings.minDistance, generationSettings.maxDistance);

        Vector3 position = new Vector3(edgePos.x + distance + sizeX / 2, edgePos.y, edgePos.z);
        Platform platform;

        if (isAnimated)
        {
            Vector3 startPosition = new Vector3(edgePos.x + distance + sizeX / 2, _startAnimationY, edgePos.z);
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

    public Platform Generate(Vector3 position, float size)
    {

        Platform platform = Instantiate(prefab, position, Quaternion.identity, _allPlatforms.transform);
        platform.GetComponent<SpriteRenderer>().size = new Vector3(size, platform.GetComponent<SpriteRenderer>().size.y);


        return platform;
    }


    private void SetStartingPlatforms()
    {
        var _currentPlatform = Generate(_firstPlatformPosition, _firstPlatformSize);
        _game._nextPlatform = Generate(_currentPlatform.GetRightEdgePosition(), _difficultyManager.GetStartingGenerationSettings(), false);
    }

    public void DestroyAllPlatforms()
    {
        var objects = GameObject.FindObjectsOfType<Platform>();
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
