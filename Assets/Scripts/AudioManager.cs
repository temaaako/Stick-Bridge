using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource _scalingSound;
    [SerializeField] private AudioSource _dropSound;
    [SerializeField] private float _startingScalingSoundPitch = 1f;
    [SerializeField] private float _stepPitch = 0.5f;
    [SerializeField] private float _fadeTime = 0.2f;

    private float _startingScalingSoundVolume;
    private IEnumerator _scalingCoroutine;

    private void Awake()
    {
        _startingScalingSoundPitch = _scalingSound.pitch;
        _startingScalingSoundVolume = _scalingSound.volume;
    }

    private void OnEnable()
    {
        EventManager.Instance.inputIsHolding += OnInputIsHolding;
        EventManager.Instance.inputEnded += OnInputEnded;
        EventManager.Instance.transitionStarted += OnTransitionStarted;
    }

    private void OnDisable()
    {
        EventManager.Instance.inputIsHolding -= OnInputIsHolding;
        EventManager.Instance.inputEnded -= OnInputEnded;
        EventManager.Instance.transitionStarted -= OnTransitionStarted;
    }


    private void OnInputEnded()
    {
        StopScalingSound();
    }

    private void OnInputIsHolding()
    {
        PlayScalingSound();
    }

    private void PlayScalingSound()
    {
        if (_scalingCoroutine != null && _scalingSound.volume != _startingScalingSoundVolume)
        {
            StopCoroutine(_scalingCoroutine);
            _scalingCoroutine= null;
            _scalingSound.Stop();
        }

        if (_scalingSound.isPlaying == false)
        {
            _scalingSound.pitch = _startingScalingSoundPitch;
            _scalingSound.volume = _startingScalingSoundVolume;
            _scalingSound.Play();
        }
        _scalingSound.pitch += _stepPitch * Time.deltaTime;
    }

    private void StopScalingSound()
    {
        _scalingCoroutine = VolumeFade(_scalingSound, 0f, _fadeTime);
        StartCoroutine(_scalingCoroutine);
    }



    private IEnumerator VolumeFade(AudioSource audioSource, float endVolume, float fadeLength)
    {

        float startVolume = audioSource.volume;

        float startTime = Time.time;

        while (Time.time < startTime + fadeLength)
        {

            audioSource.volume = startVolume + ((endVolume - startVolume) * ((Time.time - startTime) / fadeLength));

            yield return null;

        }

        if (endVolume == 0f)
        {
            audioSource.Stop();
        }

    }

    private void OnTransitionStarted(Vector3 vector)
    {
        _dropSound.Play();
    }
}

