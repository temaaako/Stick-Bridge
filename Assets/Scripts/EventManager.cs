using System;
using UnityEngine;

public class EventManager 
{
    private static EventManager instance;
    public static EventManager Instance {
        get
        {
            if (instance ==null)
            {
                instance = new EventManager();
            }
            return instance;
        }
        }

  

    public event Action<int> gameOver;
    public event Action<int> scoreChanged;
    public event Action gameRestarted;
    public event Action stickStopScaling;
    public event Action inputIsHolding;
    public event Action inputEnded;
    public event Action <Vector3> transitionStarted;
    public event Action transitionEnded;
    public event Action stickFell;


    public void GameOver(int score)
    {
        gameOver?.Invoke(score);
    }

    public void ScoreChanged(int score)
    {
        scoreChanged?.Invoke(score);
    }

    public void GameRestarted()
    {
        gameRestarted?.Invoke();
    }

    public void StickStopScaling()
    {
        stickStopScaling?.Invoke();
    }

    public void InputIsHolding()
    {
        inputIsHolding?.Invoke();
    }

    public void InputEnded()
    {
        inputEnded?.Invoke();
    }
    public void TransitionEnded()
    {
        transitionEnded?.Invoke();
    }
    public void StickFell()
    {
        stickFell?.Invoke();
    }

    public void TransitionStarted(Vector3 transitionVector)
    {
        transitionStarted?.Invoke(transitionVector);
    }
}
