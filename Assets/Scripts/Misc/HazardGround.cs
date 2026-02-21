using UnityEngine;

namespace Misc
{
    public class HazardGround : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            HealthManager.Instance.TakeDamage(1);
        }
    }
}
