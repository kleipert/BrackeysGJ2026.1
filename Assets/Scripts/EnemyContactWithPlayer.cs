using System.Collections.Generic;
using UnityEngine;

public class EnemyContactWithPlayer : MonoBehaviour
{
    [SerializeField] private int _damageToPlayer = 1;
    [SerializeField] private Collider2D _bodycollider;
    [SerializeField] private List<Collider2D> _hitColliders;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var collidersHit = _bodycollider.GetContacts(_hitColliders);
            if (collidersHit >= 1)
            {
                HealthManager.Instance.TakeDamage(_damageToPlayer);
                _hitColliders.Clear();
            }
        }
    }
}
