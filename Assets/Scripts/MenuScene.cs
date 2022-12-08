using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    private bool _isPlayScene;
    private float _loopTime;
    private Animator _animator;
    private AudioSource _audioSource;
    public AnimationClip animation;
    public AudioClip audio1;
    public AudioClip audio2;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _loopTime = animation.length;
    }

    void Update()
    {
        if (!_isPlayScene) StartCoroutine(PlayScene());
    }

    private IEnumerator PlayScene()
    {
        float loopSec = _loopTime;
        _isPlayScene = true;
        _animator.SetBool("intro", true);
        _audioSource.clip = audio1;
        _audioSource.Play();
        float audioTime = audio1.length + 2;
        loopSec -= audioTime;
        yield return new WaitForSeconds(audioTime);
        
        _audioSource.clip = audio2;
        _audioSource.Play();
        audioTime = audio2.length + 2;
        loopSec -= audioTime;
        yield return new WaitForSeconds(loopSec > audioTime ? loopSec : audioTime);
        
        _isPlayScene = false;
        _animator.SetBool("intro", false);
    }


    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
