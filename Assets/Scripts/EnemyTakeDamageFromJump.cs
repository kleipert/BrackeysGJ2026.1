using System.Collections.Generic;
using Bosses;
using UnityEngine;

public class EnemyTakeDamageFromJump : MonoBehaviour
{
    private BossHealth _bossHealth;
    [SerializeField] private Collider2D _headcollider;
    [SerializeField] private List<Collider2D> _hitColliders;
    
    // Twin jump logic
    [SerializeField] private bool _gotHit;
    [SerializeField] private bool _hasTwin;
    [SerializeField] private GameObject _twin;

    private void Start()
    {
        _bossHealth = GetComponentInParent<BossHealth>();
        _hitColliders = new List<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (_twin)
        {
            _gotHit = true;
            var twinHit = _twin.GetComponent<EnemyTakeDamageFromJump>().GotHit();
            if(twinHit)
                Destroy(transform.parent.gameObject);
        }
        else
        {
            var collidersHit = _headcollider.GetContacts(_hitColliders);
            if (collidersHit >= 1)
            {
                _bossHealth.SetCurrentHealth(_bossHealth.GetCurrentHealth() - 1);
            }
        }
    }

    public bool GotHit() => _gotHit;
}
