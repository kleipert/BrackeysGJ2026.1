using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamageFromJump : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _headcollider;
    [SerializeField] private List<Collider2D> _hitColliders;
    
    // Twin jump logic
    [SerializeField] private bool _gotHit;
    [SerializeField] private bool _hasTwin;
    [SerializeField] private GameObject _twin;

    private void Start()
    {
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
                Destroy(gameObject);
            }
        }
    }

    public bool GotHit() => _gotHit;
}
