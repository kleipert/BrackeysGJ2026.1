using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {

        [Header("Ground Check")]
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private Transform _playerGroundCheck;
        [SerializeField] private float _groundCheckDistance = .05f;
    
        [Header("Movement")]
        [SerializeField] private float _movementSpeed = 5f;
        [Range(1,2)] [SerializeField] private float _sprintMultiplier = 1.5f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _doubleJumpForce = 5f;
        [SerializeField] private bool _doubleJumpUsed;
        public bool IsGrounded { get; private set; }
        public int FacingDirection { get; private set; } = 1;
    
        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private Animator _anim;

        private bool _jumpPressed;
        [SerializeField] private bool _sprintPressed;
        private bool _facingRight = true;
    
        private static readonly int Grounded = Animator.StringToHash("isGrounded");
    
    
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
        }

        void Update()
        {
            var speed = _sprintPressed ? _movementSpeed * _sprintMultiplier : _movementSpeed;
            IsGrounded = IsPlayerOnGround();
            if (IsGrounded)
            {
                _anim.SetBool(Grounded, true);
                if(_doubleJumpUsed)
                    _doubleJumpUsed = false;
            }
            else
            {
                _anim.SetBool(Grounded, false);
            }
            _rb.linearVelocityX = _moveInput.x * speed;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            HandleFlip(_moveInput.x);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.started) return;
        
            _jumpPressed = context.started;
            if (IsGrounded && _jumpPressed)
            {
                _rb.AddForceY(_jumpForce, ForceMode2D.Impulse);
                return;
            }
            if(_jumpPressed && !_doubleJumpUsed)
            {
                _doubleJumpUsed = true;
                _rb.linearVelocityY = 0;
                _rb.AddForceY(_doubleJumpForce, ForceMode2D.Impulse);
            }
        }
    
        public void OnSprint(InputAction.CallbackContext context)
        {
            _sprintPressed = context.started;
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _facingRight = !_facingRight;
            FacingDirection *= -1;
        }
    
        private void HandleFlip(float xVelo)
        {
            if(xVelo > 0 && !_facingRight)
                Flip();
            else if (xVelo < 0 && _facingRight)
                Flip();
        }

        private bool IsPlayerOnGround()
        {
            return Physics2D.Raycast(_playerGroundCheck.position, Vector2.down, _groundCheckDistance, _whatIsGround);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_playerGroundCheck.position, new Vector2(_playerGroundCheck.position.x, _playerGroundCheck.position.y - _groundCheckDistance));
        }
    }
}
