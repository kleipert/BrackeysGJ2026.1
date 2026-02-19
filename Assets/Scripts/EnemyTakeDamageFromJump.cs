using UnityEngine;

public class EnemyTakeDamageFromJump : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
