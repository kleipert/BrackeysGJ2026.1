using UnityEngine;

public class BlutzelleMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        speed = -speed;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(speed) * Mathf.Abs(scale.x);
        transform.localScale = scale;

    }
}
