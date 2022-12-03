using System.Collections;
using GUI;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        internal int HitsNum = 3;
        internal float SonarCapacity = 100;
        private float _sonarOverload;

        internal SonarSpawner Sonar;
        private Collider _collider;
        private const float ImmunityTime = 1;
        private bool _isImmune;
        
        internal bool IsOverload;

        private IPlayerState _state;
        public IPlayerState DiedState;
        public IPlayerState SonarEnabledState;
        public IPlayerState SonarFillState;
    
        // Start is called before the first frame update
        void Start()
        {
            Sonar = GetComponent<SonarSpawner>();
            _collider = GetComponent<CapsuleCollider>();
            
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
            if ( other.CompareTag("Blade") || other.CompareTag("Bullet") && !_isImmune)
            {
                StartCoroutine(Immunity());
            }
        }

        private IEnumerator Immunity()
        {
            _isImmune = true;
            _collider.enabled = false;
            HitsNum--;
            HudController.GetInstance().RemoveHit();
            if(HitsNum == 0) ChangeState(DiedState);
            yield return new WaitForSeconds(ImmunityTime);
            _collider.enabled = true;
            _isImmune = false;
        }
    }
}
