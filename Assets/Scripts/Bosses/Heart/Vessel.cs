using System;
using UnityEngine;

public class Vessel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        {
            if (other.gameObject.CompareTag("Player"))
            {
                HealthManager.Instance.TakeDamage(1);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
