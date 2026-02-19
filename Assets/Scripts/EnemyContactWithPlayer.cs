using UnityEngine;

public class EnemyContactWithPlayer : MonoBehaviour
{
    [SerializeField] private int _damageToPlayer = 1;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            HealthManager.Instance.TakeDamage(_damageToPlayer);
    }
}
