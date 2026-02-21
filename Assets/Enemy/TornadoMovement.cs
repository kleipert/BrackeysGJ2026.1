using UnityEngine;

public class TornadoMovement : MonoBehaviour
{
    public float fallSpeed = 5f;
    public float moveSpeed = 2f;
    public LayerMask groundLayer;

    public float leftLimit = -6f;
    public float rightLimit = 6f;

    public float lifeTime = 5f; 

    private bool onGround = false;
    [HideInInspector] public int direction;

    private Rigidbody2D rb;
    private float lifeTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        lifeTimer = lifeTime; 
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        if (!onGround)
        {
            rb.linearVelocity = new Vector2(0, -fallSpeed);
        }
        else
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed, 0);

            if (transform.position.x >= rightLimit && direction > 0)
                direction = -1;

            if (transform.position.x <= leftLimit && direction < 0)
                direction = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            onGround = true;
        }
    }
}
