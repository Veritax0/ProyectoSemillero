using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Audio
{
    public class MusicController : MonoBehaviour
    {
        public List<AudioClip> clips;
        private AudioSource _audioSource;
        private bool _canChange;
        private const float Volume = 0.12f;

        private static MusicController _instance;

        public static MusicController GetInstance()
        {
            return _instance;
        }
        
        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(GetComponent<MusicController>());
        }
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = Volume;
            _canChange = true;
            //_audioSource.loop = true;
        }

        void Update()
        {
            if(_canChange) StartCoroutine(ChangeClip());
        }

        private IEnumerator ChangeClip()
        {
            _canChange = false;
            Random rn = new Random();
            AudioClip clip = clips[rn.Next(0, clips.Count)];
            _audioSource.clip = clip;
            _audioSource.Play();
            yield return new WaitForSecondsRealtime(clip.length);
            _canChange = true;
        }

        public void StopMusic()
        {
            _canChange = false;
            _audioSource.Stop();
        }
    }
}
