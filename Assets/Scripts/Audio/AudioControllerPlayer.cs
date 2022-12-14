using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Random = System.Random;

namespace Audio
{
    public class AudioControllerPlayer : MonoBehaviour
    {
        [Header("Steps")]
        public AudioSource stepsAudioSource;
        public List<AudioClip> steps;
        internal float WalkStepTime;
        internal float RunStepTime;
        internal float SquatStepTime;
        internal bool IsSquatWalk;
        internal bool IsTired;
        
        private PlayerMoveStatus _status = PlayerMoveStatus.Idle;
        
        [Header("Breathing")]
        public AudioClip slowBreath;
        public AudioClip fastBreath;
        public AudioClip tiredBreath;
        
        [Range(0,1)]
        public float squatBreathVolume;
        [Range(0,1)]
        public float walkBreathVolume;
        [Range(0,1)]
        public float runBreathVolume;
        [Range(0,1)]
        public float tiredBreathVolume;
        [Range(0,1)]
        public float idleBreathVolume;

        [Header("Hit")]
        public List<AudioClip> hurt;
        public AudioClip dying;
        [Range(0,1)]
        public float hurtVolume;
        [Range(0,1)]
        public float dieVolume;
    
        private AudioSource _audioSource;
        private Coroutine _currentCoroutine;
        private bool _isStep;
        private bool _isHurt;
        private bool _isDead;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.clip = slowBreath;
            _audioSource.volume = idleBreathVolume;
            _audioSource.Play();
        }

        private void Update()
            {
            if (_isDead || _isHurt) return;
            SetClip();
        }

        private void SetClip()
        {
            AudioClip audioClip = slowBreath;
            float volume = idleBreathVolume;
            
            switch (_status)
            {
                case PlayerMoveStatus.Walking:
                    volume = walkBreathVolume;
                    if (!_isStep)
                    {
                        StartCoroutine(Step(WalkStepTime));
                    }
                    break;
                
                case PlayerMoveStatus.Running:
                    audioClip = fastBreath;
                    volume = runBreathVolume;
                    if (!_isStep)
                    {
                        StartCoroutine(Step(RunStepTime));
                    }
                    break;
                
                case PlayerMoveStatus.Squatting:
                    volume = squatBreathVolume;
                    if (!_isStep && IsSquatWalk)
                    {
                        StartCoroutine(Step(SquatStepTime));
                    }
                    break;
                
                case PlayerMoveStatus.Dead:
                    _isDead = true;
                    _audioSource.loop = false;
                    audioClip = dying;
                    volume = dieVolume;
                    
                    stepsAudioSource.Stop();
                    stepsAudioSource.clip = null;
                    break;
                
                case PlayerMoveStatus.Idle:
                    break;
            }
            if (IsTired)
            {
                ChangeClip(tiredBreath);
                _audioSource.volume = tiredBreathVolume;
            }
            else
            {
                ChangeClip(audioClip);
                _audioSource.volume = volume;
            }
        }

        public void WalkSound()
        {
            if(!_isDead)
                _status = PlayerMoveStatus.Walking;
        }
        
        public void RunSound()
        {
            if(!_isDead)
                _status = PlayerMoveStatus.Running;
        }

        public void SquatSound()
        {
            if(!_isDead)
                _status = PlayerMoveStatus.Squatting;
        }
        
        public void IdleSound()
        {
            if(!_isDead)
                _status = PlayerMoveStatus.Idle;
        }

        public void DieSound()
        {
            _status = PlayerMoveStatus.Dead;
        }

        public void HurtSound()
        {
            StartCoroutine(Hurt());
        }

        private IEnumerator Hurt()
        {
            _isHurt = true;
            Random random = new Random();
            AudioClip audioHurt = hurt[random.Next(0, steps.Count)];
            _audioSource.clip = audioHurt;
            _audioSource.volume = hurtVolume;
            _audioSource.loop = false;
            _audioSource.Play();
            yield return new WaitForSecondsRealtime(audioHurt.length);
            _audioSource.loop = true;
            _isHurt = false;
        }
        
        private IEnumerator Step(float stepTime)
        {
            _isStep = true;
            Random random = new Random();
            stepsAudioSource.clip = steps[random.Next(0, steps.Count)];
            stepsAudioSource.Play();
            yield return new WaitForSecondsRealtime(stepTime);
            _isStep = false;
        }

        private IEnumerator Die()
        {
            
            yield return new WaitForSecondsRealtime(dying.length);
        }

        private void ChangeClip(AudioClip clip)
        {
            if (_audioSource.clip == clip) return;
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
