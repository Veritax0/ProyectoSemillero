using System.Security.Cryptography;
using UnityEngine;

namespace Player
{
    public class PlayerDiedState : MonoBehaviour, IPlayerState
    {
        private PlayerController _context;
        private PlayerMovement _movement;
        private Rigidbody _rb;
        public void Execute() 
        {
            _movement.enabled = false;
            Destroy(_rb);
        }

        public void SetContext(PlayerController context)
        {
            _context = context;
            _movement = GetComponent<PlayerMovement>();
            _rb = GetComponent<Rigidbody>();
        }
    }
}