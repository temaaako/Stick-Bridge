using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
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
