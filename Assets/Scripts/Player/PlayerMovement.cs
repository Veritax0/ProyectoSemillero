using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed;
        public Transform orientation;

        //Movement
        private bool _isRun;
        private bool _isSquat;
        private float _originalMoveSpeed;
        private float _horizontalInput;
        private float _verticalInput;
        private Vector3 _moveDirection;
        private Rigidbody _rb;
        private float _runSpeed;
        private float _squatSpeed;
        private const float RunSpeedFactor = 1.3f;
        private const float SquatSpeedFactor = 0.6f;

        private float _runCapacity = 100;
        private const float RunDecreasePerSec = 20;
        private const float RunLoadTime = 10;
        private const float MinCapacityToRun = 30;
        private bool _isAbleToRun = true;
        private bool _isMove;

        //Scale
        private Vector3 _originalScale;
        private float _originalColliderRadius;
        private float _runColliderRadius;
        private Vector3 _squatScale;
        private CapsuleCollider _collider;
        private const float SquatScaleYFactor = 0.6f;
        private const float ColliderRadiusFactor = 1.6f;
        

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
            _originalColliderRadius = _collider.radius;
            _rb.freezeRotation = true;
            
            _originalMoveSpeed = moveSpeed;
            _runSpeed = _originalMoveSpeed * RunSpeedFactor;
            _squatSpeed = _originalMoveSpeed * SquatSpeedFactor;

            _originalScale = transform.localScale;
            _runColliderRadius = _originalColliderRadius * ColliderRadiusFactor;
            _squatScale = _originalScale;
            _squatScale.y = _originalScale.y * SquatScaleYFactor;
        }

        private void Update()
        {
            UpdateInput();
            if (_isRun)
            {
                DecreaseRun();
            }
            else
            {
                IncreaseRun();
            }
        }


        private void FixedUpdate()
        {
            MovePlayer();
        }
        private void IncreaseRun()
        {
            if(_runCapacity >= 100)return;
            
            float frameTime = Time.fixedDeltaTime;
            float increase = 100 / RunLoadTime * frameTime;
            float newCapacity = _runCapacity + increase;
            _runCapacity = newCapacity <= 100 ? newCapacity : 100;

            _isAbleToRun = _runCapacity >= MinCapacityToRun;
        }

        private void DecreaseRun()
        {
            float frameTime = Time.fixedDeltaTime;
            float decrease = RunDecreasePerSec * frameTime;
            _runCapacity = _runCapacity >= decrease ? _runCapacity - decrease : 0;
            if(_runCapacity == 0) _isAbleToRun = false;
        }

        private void UpdateInput()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");
            _isMove = _horizontalInput != 0 || _verticalInput != 0;
            
            //Squatting
            if (Input.GetKey(KeyCode.LeftControl))
            {
                moveSpeed = _squatSpeed;
                _isSquat = true;
                _isRun = false;
                Transform tf = transform;
                tf.localScale = _squatScale;
                Vector3 pos = tf.position;
                pos.y = _squatScale.y;
                tf.position = pos;
                _collider.height = _squatScale.y;
            }
            //Quit squatting
            else if(Input.GetKeyUp(KeyCode.LeftControl))
            {
                moveSpeed = _originalMoveSpeed;
                _isSquat = false;
                Transform tf = transform;
                tf.localScale = _originalScale;
                Vector3 pos = tf.position;
                pos.y = _originalScale.y;
                tf.position = pos;
                _collider.height = _originalScale.y;
            }
            //Running
            else if (Input.GetKey(KeyCode.LeftShift) && _isAbleToRun && _isMove)
            {
                moveSpeed = _runSpeed;
                _isRun = true;
                _collider.radius = _runColliderRadius;
            }
            //Quit running
            else if(Input.GetKeyUp(KeyCode.LeftShift) || !_isAbleToRun || !_isMove)
            {
                moveSpeed = _originalMoveSpeed;
                _isRun = false;
                _collider.radius = _originalColliderRadius;
            }
        }

        private void MovePlayer()
        {
            _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
            _rb.AddForce(_moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }

        public PlayerMoveStatus GetMovStatus()
        {
            if (!_isRun && !_isSquat)
            {
                return PlayerMoveStatus.Normal;
            }
            return _isSquat ? PlayerMoveStatus.Squatting : PlayerMoveStatus.Running;
        }
    }
}