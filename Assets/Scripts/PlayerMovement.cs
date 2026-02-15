using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private bool _doubleJumpUsed = false;
    public bool IsGrounded { get; private set; }
    
    private Rigidbody2D _rb;
    private Vector2 moveInput;

    private bool _jumpPressed = false;
    [SerializeField] private bool _sprintPressed = false;
    private bool _facingRight = true;
    public int FacingDirection { get; private set; } = 1;
    
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var speed = _sprintPressed ? _movementSpeed * _sprintMultiplier : _movementSpeed;
        IsGrounded = IsPlayerOnGround();
        if (IsGrounded && _doubleJumpUsed)
            _doubleJumpUsed = false;
        _rb.linearVelocityX = moveInput.x * speed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        HandleFlip(moveInput.x);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _jumpPressed = context.performed;
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
        _sprintPressed = context.performed;
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
