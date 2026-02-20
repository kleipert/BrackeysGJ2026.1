using UnityEngine;

public class CreateLaser : MonoBehaviour
{
    [SerializeField] private GameObject laser;

    public void Create()
    {
        if(!laser) return;
        laser.gameObject.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            HealthManager.Instance.TakeDamage(2);
        }
    }
}
