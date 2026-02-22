using UnityEngine;

public class TornadoMovement : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private float leftLimit = -6f;
    [SerializeField] private float rightLimit = 6f;

    [SerializeField] private float lifeTime = 5f; 

    [SerializeField]private bool onGround = false;
    public int direction = 1;

    private Rigidbody2D rb;
    private float lifeTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.gravityScale = 1;

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
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime * direction);

            if (transform.position.x >= rightLimit && direction > 0)
                direction = -1;

            if (transform.position.x <= leftLimit && direction < 0)
                direction = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
        {
            onGround = true;

        }

    }
}
