using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        {
            if (other.gameObject.CompareTag("Player"))
            {
                HealthManager.Instance.TakeDamage(1);
            }
        }
    }
}
