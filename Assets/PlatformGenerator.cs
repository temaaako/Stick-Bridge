using DG.Tweening;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    [SerializeField] private Platform prefab;

    [SerializeField] private float _platformMinSize=1f;
    [SerializeField] private float _platformMaxSize=3f;

    [SerializeField] private float _platformMinDistance = 1f;
    [SerializeField] private float _platformMaxDistance = 3f;

    [SerializeField] private float _moveUpDuration=0.2f;

    [SerializeField] private float _startAnimationY = -10f;

    private Game _game;

    private AllPlatforms allPlatforms;
    private void Awake()
    {
         allPlatforms=FindObjectOfType<AllPlatforms>();
        _game = FindObjectOfType<Game>();
    }

    public Platform Generate(Vector3 edgePos, bool isAnimated)
    {
        float size = Random.Range(_platformMinSize, _platformMaxSize);
        float distance = Random.Range(_platformMinDistance, _platformMaxDistance);
        Vector3 position = new Vector3(edgePos.x + distance + size / 2, edgePos.y, edgePos.z);
        Platform platform;

        if (isAnimated)
        {
            Vector3 startPosition = new Vector3(edgePos.x + distance + size / 2, _startAnimationY, edgePos.z);
            platform = Instantiate(prefab, startPosition, Quaternion.identity, allPlatforms.transform);
            platform.transform.DOMoveY(position.y, _moveUpDuration).SetEase(Ease.InSine);
        }
        else
        {
            platform = Instantiate(prefab, position, Quaternion.identity, allPlatforms.transform);
        }

        platform.GetComponent<SpriteRenderer>().size = new Vector3(size, platform.GetComponent<SpriteRenderer>().size.y);

        return platform;
    }

    public Platform Generate(Vector3 position, float size)
    {

        Platform platform = Instantiate(prefab, position, Quaternion.identity, allPlatforms.transform);
        platform.GetComponent<SpriteRenderer>().size = new Vector3(size, platform.GetComponent<SpriteRenderer>().size.y);


        return platform;
    }



}
