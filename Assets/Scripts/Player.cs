using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventManager.Instance.transitionStarted += OnTransitionStarted;
        EventManager.Instance.transitionEnded += OnTransitionEnded;

    }
    private void OnDisable()
    {
        EventManager.Instance.transitionStarted -= OnTransitionStarted;
        EventManager.Instance.transitionEnded -= OnTransitionEnded;
    }

    private void OnTransitionEnded()
    {
        Idle();
    }


    private void OnTransitionStarted(Vector3 transition)
    {
        Run();
    }




    public void Run()
    {
        animator.SetBool("Run", true);
    }

    public void Idle()
    {
        animator.SetBool("Run", false);
    }
}
