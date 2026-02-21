using System;
using UnityEngine;

public class DeathWalll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            HealthManager.Instance.TakeDamage(7);
    }
}
