using System;
using UnityEngine;

namespace Misc
{
    public class HealthPickup : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.up, 180 * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            HealthManager.Instance.IncreaseHealth();
            HealthManager.Instance.Heal(10);
            Destroy(gameObject);
        }
    }
}
