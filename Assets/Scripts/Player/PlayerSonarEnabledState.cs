using GUI_;
using UnityEngine;

namespace Player
{
    public class PlayerSonarEnabledState : MonoBehaviour, IPlayerState
    {
        private PlayerController _context;
        private float _sonarOverload;
        private const float SonarDecreasePerSec = 20;
        private const float MaxOverload = 30;
        private float _spaceTime;
        public void Execute()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _spaceTime = 0;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (_spaceTime <= 0.6)
                {
                    _context.SonarCapacity -= SonarDecreasePerSec / 2;
                }
                _spaceTime = 0;
            }
            
            if (Input.GetKey(KeyCode.Space) && !_context.IsOverload)
            {
                _context.Sonar.Scan();
                float frameTime = Time.fixedDeltaTime;
                _spaceTime += frameTime;
                float decrease = SonarDecreasePerSec * frameTime;
                float sonarCapacity = _context.SonarCapacity - decrease;
                sonarCapacity = sonarCapacity >= 0 ? sonarCapacity : 0;
                _context.SonarCapacity = sonarCapacity;
                HudController.GetInstance().UpdateSonarBar(sonarCapacity);
                
                if (sonarCapacity == 0)
                {
                    _sonarOverload += _sonarOverload + decrease <= MaxOverload ? decrease : MaxOverload;
                    HudController.GetInstance().UpdateInverseOverload(_sonarOverload, MaxOverload);
                    if (_sonarOverload >= MaxOverload)
                    {
                        _sonarOverload = 0;
                        _context.IsOverload = true;
                        _context.ChangeState( _context.HitsNum == 0 ? _context.DiedState : _context.SonarFillState);
                    }
                }
            }
            else
            {
                _context.ChangeState(_context.SonarFillState);
            }
            
        }

        public void SetContext(PlayerController context)
        {
            _context = context;
        }
    }
}