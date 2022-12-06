using System.Collections;
using Audio;
using GUI_;
using Sonar;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        internal int HitsNum = 3;
        internal float SonarCapacity = 100;
        private float _sonarOverload;

        internal SonarSpawner Sonar;
        private const float ImmunityTime = 1;
        private bool _isImmune;
        
        public float knockBack;
        internal bool IsOverload;
        internal AudioControllerPlayer AudioPlayer;

        private IPlayerState _state;
        internal IPlayerState DiedState;
        internal IPlayerState SonarEnabledState;
        internal IPlayerState SonarFillState;
        void Start()
        {
            Sonar = GetComponent<SonarSpawner>();
            ScannerControllerPlayer.SetPlayerTransform(transform);
            AudioPlayer = GetComponent<AudioControllerPlayer>();

            DiedState = gameObject.AddComponent<PlayerDiedState>();
            DiedState.SetContext(this);
            SonarEnabledState = gameObject.AddComponent<PlayerSonarEnabledState>();
            SonarEnabledState.SetContext(this);
            SonarFillState = gameObject.AddComponent<PlayerSonarFillState>();
            SonarFillState.SetContext(this);
        
            _state = SonarEnabledState;
        }

        private void FixedUpdate()
        {
            _state.Execute();
        }

        public void ChangeState(IPlayerState state)
        {
            _state = state;
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if ( !_isImmune && (other.CompareTag("Blade") || other.CompareTag("Bullet") ) )
            {
                Vector3 pos = transform.position;
                pos.y = 0;
                Vector3 otherPos = other.ClosestPoint(pos);
                otherPos.y = 0;
                Vector3 dir = Vector3.Normalize(pos - otherPos);
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.AddForce(dir * knockBack, ForceMode.Impulse);
                StartCoroutine(Immunity());
            }
        }

        private IEnumerator Immunity()
        {
            _isImmune = true;
            HitsNum--;
            HudController.GetInstance().RemoveHit();
            if (HitsNum <= 0)
            {
                ChangeState(DiedState);
            }
            else
            {
                AudioPlayer.HurtSound();
            }
            yield return new WaitForSeconds(ImmunityTime);
            _isImmune = false;
        }
    }
}
