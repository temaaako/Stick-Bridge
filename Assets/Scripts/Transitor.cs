using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Transitor : MonoBehaviour
{
    [SerializeField] private float _transitionTime = 2;

    private void OnEnable()
    {
        EventManager.Instance.transitionStarted += OnTransitionStarted;
    }

    private void OnDisable()
    {
        EventManager.Instance.transitionStarted -= OnTransitionStarted;
    }

    public void MakeTransition(float xDistance)
    {
        
    }

    private void OnValidate()
    {
        if (_transitionTime < 0)
        {
            _transitionTime = 0;
        }
    }



    private void OnTransitionStarted(Vector3 transition)
    {
        StartCoroutine(MakeTransition(transition));
    }

    private IEnumerator MakeTransition(Vector3 transition)
    {
        transform.DOMove(transform.position + transition, _transitionTime);
        yield return new WaitForSeconds(_transitionTime);
        EventManager.Instance.TransitionEnded();
    }
}
