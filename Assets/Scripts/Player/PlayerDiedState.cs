using Audio;
using GUI_;
using UnityEngine;

namespace Player
{
    public class PlayerDiedState : MonoBehaviour, IPlayerState
    {
        private PlayerController _context;
        private PlayerMovement _movement;
        private Rigidbody _rb;
        private bool _soundDone;
        public void Execute() 
        {
            _movement.SetDied();
            _movement.enabled = false;
            Destroy(_rb);
            MusicController.GetInstance().StopMusic();
            if(!_soundDone){
                _context.AudioPlayer.DieSound();
                _soundDone = true;
            }
            HudController.GetInstance().Defeat();
        }

        public void SetContext(PlayerController context)
        {
            _context = context;
            _movement = GetComponent<PlayerMovement>();
            _rb = GetComponent<Rigidbody>();
        }
    }
}