using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    private float _size;


    public Vector3 GetRightEdgePosition()
    {
        return transform.position + new Vector3(GetComponent<SpriteRenderer>().size.x / 2, 0, 0);
    }
}
