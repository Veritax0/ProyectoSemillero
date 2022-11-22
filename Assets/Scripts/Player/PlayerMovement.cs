using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed;

        public Transform orientation;

        private float _horizontalInput;
        private float _verticalInput;
        private Vector3 _moveDirection;
        private Rigidbody _rb;
        private CharacterController _character;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            _character = GetComponent<CharacterController>();
        }

        private void Update()
        {
            UpdateInput();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void UpdateInput()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical"); 
        }

        private void MovePlayer()
        {
            _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
            _character.Move(_moveDirection * (moveSpeed * Time.deltaTime));
        }
    }
}