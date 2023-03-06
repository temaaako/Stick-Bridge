using UnityEngine;

public class Platform : MonoBehaviour
{

    public Vector3 GetRightEdgePosition()
    {
        return transform.position + new Vector3(GetComponent<SpriteRenderer>().size.x / 2, 0, 0);
    }

    public float GetSizeX()
    {
        return GetComponent<SpriteRenderer>().size.x;
    }
}
