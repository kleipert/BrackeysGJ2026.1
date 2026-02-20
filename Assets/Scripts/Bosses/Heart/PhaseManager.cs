using System.Collections;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    [SerializeField] private GameObject phase1;
    [SerializeField] private GameObject phase2;
    [SerializeField] private GameObject phase3;
    [SerializeField] private GameObject shield;
    [SerializeField] private Transform heartCenter;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float ejectRadius = 3f;
    [SerializeField] private float ejectOutForce = 8f;     
    [SerializeField] private float ejectUpForce = 6f;    
    [SerializeField] private float shieldColliderDelay = 0.15f;
    [SerializeField] private int healthHeart = 3;

    public static PhaseManager Instance { get; private set; }

    private int _currentPhaseIndex = 0;
    private bool _waitingForHeartDamage = false;

    private Collider2D shieldCollider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (!shieldCollider && shield)
            shieldCollider = shield.GetComponent<Collider2D>();
    }

    private void Start()
    {
        phase1.SetActive(true);
        phase2.SetActive(false);
        phase3.SetActive(false);

        _currentPhaseIndex = 0;
        _waitingForHeartDamage = false;

        SetShieldActive(true, immediateCollider: true);
    }

    private void Update()
    {
        if (!_waitingForHeartDamage && AreAllDirectChildrenInactive(GetCurrentPhase()))
        {
            SetShieldActive(false, immediateCollider: true);
            _waitingForHeartDamage = true;
        }
    }


    public void DamageHeart()
    {
        if (!_waitingForHeartDamage)
            return;

        healthHeart = Mathf.Max(0, healthHeart - 1);
        
        if (_currentPhaseIndex < 2)
        {
            GameObject from = GetCurrentPhase();
            _currentPhaseIndex++;
            GameObject to = GetCurrentPhase();

            SwitchPhase(from, to);
            
            SetShieldActive(true, immediateCollider: false);
            EjectPlayersFromHeart2D();
            StartCoroutine(EnableShieldColliderDelayed());

            _waitingForHeartDamage = false;
        }
        else
        {
            SetShieldActive(false, immediateCollider: true);
        }
    }

    private void SwitchPhase(GameObject from, GameObject to)
    {
        if (from) from.SetActive(false);
        if (to) to.SetActive(true);
    }

    private GameObject GetCurrentPhase()
    {
        return _currentPhaseIndex switch
        {
            0 => phase1,
            1 => phase2,
            _ => phase3,
        };
    }

    private void SetShieldActive(bool active, bool immediateCollider)
    {
        if (!shield) return;

        shield.SetActive(active);

        if (!shieldCollider && shield)
            shieldCollider = shield.GetComponent<Collider2D>();

        if (!shieldCollider) return;

        if (!active)
        {
            shieldCollider.enabled = false;
        }
        else
        {
            shieldCollider.enabled = immediateCollider;
        }
    }

    private bool AreAllDirectChildrenInactive(GameObject parent)
    {
        if (!parent) return true;

        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.activeInHierarchy)
                return false;
        }
        return true; 
    }

    private void EjectPlayersFromHeart2D()
    {
        if (!heartCenter) return;

        Vector2 heartPos = heartCenter.position;

        // 2D-Overlap!
        Collider2D[] hits = Physics2D.OverlapCircleAll(heartPos, ejectRadius, playerMask);

        foreach (var h in hits)
        {
            if (!h || h.isTrigger) continue;

            Rigidbody2D rb = h.attachedRigidbody;
            if (!rb) continue;

            // Richtung: vom Herz weg + nach oben
            Vector2 dir = (rb.position - heartPos);
            if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;
            dir = dir.normalized;

            Vector2 impulse = dir * ejectOutForce + Vector2.up * ejectUpForce;

            rb.linearVelocity = Vector2.zero;                 
            rb.AddForce(impulse, ForceMode2D.Impulse);  
        }
    }

    private IEnumerator EnableShieldColliderDelayed()
    {
        if (!shieldCollider) yield break;

        shieldCollider.enabled = false;
        yield return new WaitForSeconds(shieldColliderDelay);
        shieldCollider.enabled = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!heartCenter) return;
        Gizmos.DrawWireSphere(heartCenter.position, ejectRadius);
    }
#endif
}