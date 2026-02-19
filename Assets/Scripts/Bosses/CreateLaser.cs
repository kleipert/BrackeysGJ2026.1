using UnityEngine;

public class CreateLaser : MonoBehaviour
{
    [SerializeField] private GameObject laser;

    public void Create()
    {
        if(!laser) return;
        laser.gameObject.SetActive(true);
    }
}
