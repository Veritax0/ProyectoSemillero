using UnityEngine;

namespace Player
{
    public class PlayerSonarFillState : MonoBehaviour, IPlayerState
    {
        private PlayerController _context;
        private const float LoadTime = 5;
        
        public void Execute()
        {
            /*if (Input.GetKey(KeyCode.Space))
            {
                _context.ChangeState(_context.SonarEnabledState);
                return;
            }*/

            float sonarCapacity = _context.SonarCapacity;
            if (sonarCapacity >= 100)
            {
                _context.IsOverload = false;
                _context.ChangeState(_context.SonarEnabledState);
                return;
            }
            float frameTime = Time.fixedDeltaTime;
            float increase = 100 / LoadTime * frameTime;
            float newCapacity = sonarCapacity + increase;
            _context.SonarCapacity = newCapacity < 100 ? newCapacity : 100;
        }

        public void SetContext(PlayerController context)
        {
            _context = context;
        }
    }
}