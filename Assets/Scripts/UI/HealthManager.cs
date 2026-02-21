using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image[] healthBar;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip playerDeath;

    private int currentHealth;
    private int maxHealth;
    private bool isInvincible;

    public static HealthManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        currentHealth = 3;
        maxHealth = 3;
        
        for (int i = maxHealth; i < healthBar.Length; i++)
        {
            if (healthBar[i] != null)
                healthBar[i].enabled = false;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void TakeDamage(int amount)
    {
        if (isInvincible)  return;
        
        SoundManager.Instance.PlaySound(playerHit,player.transform,0.3f);
        
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        
        for (int i = 0; i < maxHealth; i++)
        {
            if (healthBar[i] == null) continue;

            var anim = healthBar[i].GetComponent<Animator>();
            if (anim != null)
                anim.SetBool("Lost", i >= currentHealth);
        }

        if (currentHealth <= 0)
        {
            PauseManager.Instance.IsDead();
            SoundManager.Instance.PlaySound(playerDeath,player.transform,0.3f,5f);
        }
        
        StartCoroutine(Invincible());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        for (int i = 0; i < maxHealth; i++)
        {
            if (healthBar[i] == null) continue;

            var anim = healthBar[i].GetComponent<Animator>();
            if (anim != null)
                anim.SetBool("Lost", i >= currentHealth);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void IncreaseHealth()
    {
        if (maxHealth >= healthBar.Length) return;
        
        if (healthBar[maxHealth] != null)
            healthBar[maxHealth].enabled = true;

        maxHealth++;
        currentHealth = maxHealth;
        
        for (int i = 0; i < maxHealth; i++)
        {
            if (healthBar[i] == null) continue;

            var anim = healthBar[i].GetComponent<Animator>();
            if (anim != null)
                anim.SetBool("Lost", false);
        }
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3f);
        isInvincible = false;
    }
}
