using UnityEngine;

namespace Player
{
    public class PlayerSonarEnabledState : MonoBehaviour, IPlayerState
    {
        private PlayerController _context;
        private float _sonarOverload;
        private const float SonarDecreasePerSec = 10;
        private const float MaxOverload = 30;
        public void Execute()
        {
            if (Input.GetKey(KeyCode.Space) && !_context.IsOverload)
            {
                _context.Sonar.Scan();
                float frameTime = Time.fixedDeltaTime;
                float decrease = SonarDecreasePerSec * frameTime;
                float sonarCapacity = _context.SonarCapacity;
                _context.SonarCapacity = sonarCapacity >= decrease ? sonarCapacity - decrease : 0;
                if (sonarCapacity == 0)
                {
                    _sonarOverload += _sonarOverload + decrease <= MaxOverload ? decrease : MaxOverload;
                    if (_sonarOverload >= MaxOverload)
                    {
                        _context.HitsNum--;
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