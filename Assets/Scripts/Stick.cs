using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{

    private float _width;
    public float Width => _width;

    private float _startingLength;
    public float StartingLength => _startingLength;

    private float _startingWidth;

    private void Start()
    {
        _width = transform.GetChild(0).transform.localScale.x;
        _startingWidth = transform.localScale.x;
    }

    public float GetLength()
    {
        return transform.transform.localScale.y;
    }


    public void SetStartingSize()
    {
        transform.transform.localScale = new Vector3(_startingWidth, StartingLength);
    }


}
