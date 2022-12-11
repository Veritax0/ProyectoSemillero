using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Audio
{
    public class AudioControllerEnemy : MonoBehaviour
    {
        public List<AudioClip> steps;
        public List<AudioClip> attackSounds;

        private bool _isWalk;
        private bool _isStep;
        private bool _isAttack;
        
        private AudioSource _audioSource;
        public float stepTime = 0.4f;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if(_isAttack) return;
            if (_isWalk && !_isStep && Time.timeScale > 0) StartCoroutine(Step());
        }

        private IEnumerator Step()
        {
            _isStep = true;
            Random random = new Random();
            _audioSource.clip = steps[random.Next(0, steps.Count)];
            _audioSource.Play();
            yield return new WaitForSecondsRealtime(stepTime);
            _isStep = false;
        }

        private IEnumerator Attack()
        {
            _isAttack = true;
            Random random = new Random();
            AudioClip clip = attackSounds[random.Next(0, attackSounds.Count)];
            _audioSource.clip = clip;
            _audioSource.Play();
            yield return new WaitForSecondsRealtime(clip.length);
            _isAttack = false;
        }
        
        public void Walk()
        {
            _isWalk = true;
        }

        public void Idle()
        {
            _isWalk = false;
        }

        public void AttackSound()
        {
            StartCoroutine(Attack());
        }
    }
}