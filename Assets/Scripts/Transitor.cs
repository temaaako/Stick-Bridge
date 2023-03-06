using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitor : MonoBehaviour
{
    [SerializeField] private float duration;

    public void MakeTransition(float xDistance)
    {
        
    }

    private void OnValidate()
    {
        if (duration<0)
        {
            duration = 0;
        }
    }
}
