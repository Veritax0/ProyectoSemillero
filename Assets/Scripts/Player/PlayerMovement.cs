using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed;
        public Transform orientation;

        private const float RunSpeedFactor = 1.3f;
        private float _originalMoveSpeed;
        private float _horizontalInput;
        private float _verticalInput;
        private Vector3 _moveDirection;
        private Rigidbody _rb;
        private float _runSpeed;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            _originalMoveSpeed = moveSpeed;
            _runSpeed = _originalMoveSpeed * RunSpeedFactor;
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
            if (Input.GetKey(KeyCode.LeftControl))
            {
                moveSpeed = _runSpeed;
            }
            else if(Input.GetKeyUp(KeyCode.LeftControl))
            {
                moveSpeed = _originalMoveSpeed;
            }
        }

        private void MovePlayer()
        {
            _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
            _rb.AddForce(_moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }
    }
}