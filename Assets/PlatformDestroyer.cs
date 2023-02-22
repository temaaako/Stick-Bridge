using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    // Start is called before the first frame update

    Game game;

    void Start()
    {

        game = FindObjectOfType<Game>();
        game.gameReset += OnGameReset;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }


   


    private void OnGameReset()
    {

    }
}
