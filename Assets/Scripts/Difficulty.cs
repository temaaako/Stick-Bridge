using UnityEngine;

public class Difficulty : MonoBehaviour
{
    [SerializeField] private string _name;


    [SerializeField] private float _platformMinSize = 1f;
    [SerializeField] private float _platformMaxSize = 3f;

    [SerializeField] private float _platformMinDistance = 1f;
    [SerializeField] private float _platformMaxDistance = 3f;

    public PlatformGenerationSettings GetGenerationSettings()
    {
        return new PlatformGenerationSettings(_platformMinSize, _platformMaxSize, _platformMinDistance, _platformMaxDistance);
    }


    [SerializeField] private GameObject startPointObject;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 rightEdge = startPointObject.transform.position + new Vector3(startPointObject.GetComponent<SpriteRenderer>().size.x / 2, startPointObject.GetComponent<SpriteRenderer>().size.y / 2, 0);
        Gizmos.DrawSphere(rightEdge, 0.05f);
        Gizmos.DrawLine(rightEdge + new Vector3(_platformMinDistance, 0, 0), rightEdge + new Vector3(_platformMaxDistance + _platformMaxSize, 0, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(rightEdge + new Vector3(_platformMinDistance, 0.2f, 0), rightEdge + new Vector3(_platformMinDistance + _platformMaxSize, 0.2f, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rightEdge + new Vector3(_platformMinDistance, 0.4f, 0), rightEdge + new Vector3(_platformMinDistance + _platformMinSize, 0.4f, 0));
    }

}
