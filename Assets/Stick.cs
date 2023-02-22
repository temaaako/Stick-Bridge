using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public float GetLength()
    {
        return transform.GetChild(0).transform.localScale.y;
    }
}
