using GUI;
using UnityEngine;

namespace Player
{
    public class PlayerSonarFillState : MonoBehaviour, IPlayerState
    {
        private PlayerController _context;
        private const float LoadTime = 8;
        
        public void Execute()
        {
            if (Input.GetKey(KeyCode.Space) && !_context.IsOverload)
            {
                _context.ChangeState(_context.SonarEnabledState);
                return;
            }

            float sonarCapacity = _context.SonarCapacity;
            if(_context.IsOverload) HudController.GetInstance().UpdateOverload(sonarCapacity);
            
            if (sonarCapacity >= 100)
            {
                _context.IsOverload = false;
                _context.ChangeState(_context.SonarEnabledState);
                return;
            }
            float frameTime = Time.fixedDeltaTime;
            float increase = 100 / LoadTime * frameTime;
            sonarCapacity += increase;
            sonarCapacity = sonarCapacity < 100 ? sonarCapacity : 100;
            _context.SonarCapacity = sonarCapacity;
            HudController.GetInstance().UpdateSonarBar(sonarCapacity);
        }

        public void SetContext(PlayerController context)
        {
            _context = context;
        }
    }
}