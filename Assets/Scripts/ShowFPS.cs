using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;

public class ShowFPS : MonoBehaviour
{
    public TMP_Text fpsText;
    public TMP_Text minFpsText;
    public float deltaTime;

    public float minimalFpsTime = 5f;

    private float _timeElapsed = 0;
    private void Start()
    {
        minFpsText.text = "70";
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed>=minimalFpsTime)
        {
            minFpsText.text = fpsText.text;
            _timeElapsed = 0;
        }
        if (Convert.ToInt32(minFpsText.text)> Convert.ToInt32(fpsText.text))
        {
            minFpsText.text = fpsText.text;
            _timeElapsed= 0;
        }
    }
}