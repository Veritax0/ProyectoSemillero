using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AudioController : MonoBehaviour
{
    public List<AudioClip> enemySteps;
    public List<AudioClip> playerSteps;
    private AudioSource _audioSource;
    private bool _isStep;
    public float stepTime = 0.5f;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_isStep) StartCoroutine(Step());
    }

    private IEnumerator Step()
    {
        _isStep = true;
        Random random = new Random();
        _audioSource.clip = playerSteps[random.Next(0, playerSteps.Count)];
        _audioSource.Play();
        yield return new WaitForSecondsRealtime(stepTime);
        _isStep = false;
    }
}
