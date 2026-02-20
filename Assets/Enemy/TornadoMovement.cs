using UnityEngine;

public class TornadoMovement : MonoBehaviour
{
    public float fallSpeed = 5f;
    public float moveSpeed = 2f;
    public LayerMask groundLayer;

    private bool onGround = false;
    private int direction = 1;

    void Update()
    {
        if (!onGround)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            onGround = true;
        }
    }
}
