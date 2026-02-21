using System;
using UnityEngine;

public class Vessel : MonoBehaviour
{
    [SerializeField] private AudioClip _audioclip;
    
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
                SoundManager.Instance.PlaySound(_audioclip, transform, 0.3f);
            }
        }
    }
}
