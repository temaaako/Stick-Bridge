
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{

    private void OnEnable()
    {
        EventManager.Instance.gameRestarted += OnGameReset;
    }

    private void OnDisable()
    {
        EventManager.Instance.gameRestarted -= OnGameReset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }


    private void OnGameReset()
    {

    }
}
