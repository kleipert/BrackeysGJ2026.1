using UnityEngine;

public class EnemyBaseMovement : MonoBehaviour
{
    
    protected Rigidbody2D Rb { get; private set; }
    
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected float _wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;

    [SerializeField] protected float movementSpeed;
    
    protected bool GroundDetected { get; set; }
    protected bool WallDetected { get; set; }
    
    protected bool EnemyDetected { get; set; }

    private bool _facingRight = true;
    protected int FacingDirection { get; set; } = 1;
    
    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        HandleEnemyCollision();
        HandleGroundCollision();
        SetVelocity(FacingDirection * movementSpeed, Rb.linearVelocity.y);
        if(WallDetected || !GroundDetected || EnemyDetected)
            Flip();
    }
    
    private void HandleGroundCollision()
    {
        GroundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, _groundCheckDistance, whatIsGround);

        WallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection,
            _wallCheckDistance, whatIsGround);
            
    }

    protected virtual void HandleEnemyCollision()
    {
        EnemyDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection,
            .5f, whatIsEnemy);
    }
    
    public void SetVelocity(float xVelo, float yVelo)
    {
        Rb.linearVelocity = new Vector2(xVelo, yVelo);
        HandleFlip(xVelo);
    }
    
    protected void Flip()
    {
        transform.Rotate(0, 180, 0);
        _facingRight = !_facingRight;
        FacingDirection *= -1;
    }
    
    protected void HandleFlip(float xVelo)
    {
        if(xVelo > 0 && !_facingRight)
            Flip();
        else if (xVelo < 0 && _facingRight)
            Flip();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -_groundCheckDistance, 0));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(_wallCheckDistance * FacingDirection, 0, 0));
    }
}
