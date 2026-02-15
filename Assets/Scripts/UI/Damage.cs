using System.Collections;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Damage : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Hit());
        StartCoroutine(Hit());
        StartCoroutine(Heal());
        StartCoroutine(Increase());
    }

    IEnumerator Hit()
    {
        yield return new WaitForSeconds(2f);
        HealthManager.Instance.TakeDamage(1);
    }

    IEnumerator Heal()
    {
        yield return new WaitForSeconds(4f);
        HealthManager.Instance.Heal(1);
    }

    IEnumerator Increase()
    {
        yield return new WaitForSeconds(6f);
        HealthManager.Instance.IncreaseHealth();
    }
}
