using System.Collections;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private static readonly int IsVulnerable = Animator.StringToHash("IsVurneable");

    [Header("Phases (size 3 in Inspector)")]
    [SerializeField] private GameObject[] phases = new GameObject[3];

    [Header("Shield")]
    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldActivateDelay = 0.15f;

    [Header("Heart / End")]
    [SerializeField] private int heartHealth = 3;
    [SerializeField] private GameObject exitZone;
    [SerializeField] private GameObject heartObject;

    [Header("Vulnerability / Animation")]
    [SerializeField] private Animator animator;

    [Header("Player Eject")]
    [SerializeField] private Transform heartCenter;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float ejectRadius = 3f;
    [SerializeField] private float ejectOutForce = 8f;
    [SerializeField] private float ejectUpForce = 6f;

    public static PhaseManager Instance { get; private set; }

    private int _phaseIndex;
    private bool _waitingForHeartDamage;

    private Collider2D _shieldCollider;
    private Coroutine _shieldRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (shield) _shieldCollider = shield.GetComponent<Collider2D>();
    }

    private void Start()
    {
        for (int i = 0; i < phases.Length; i++)
            if (phases[i]) phases[i].SetActive(i == 0);

        _phaseIndex = 0;
        _waitingForHeartDamage = false;

        SetShieldActive(true);
        SetVulnerable(false);

        if (exitZone) exitZone.SetActive(false);
    }

    private void Update()
    {
        var current = GetCurrentPhase();
        if (!_waitingForHeartDamage && AreAllDirectChildrenInactive(current))
        {
            SetShieldActive(false);
            SetVulnerable(true);
            _waitingForHeartDamage = true;
        }
    }

    public void DamageHeart()
    {
        if (!_waitingForHeartDamage)
            return;

        _waitingForHeartDamage = false;
        SetVulnerable(false);

        heartHealth = Mathf.Max(0, heartHealth - 1);
        
        EjectPlayersFromHeart2D();
        
        if (heartHealth <= 0)
        {
            EndFight();
            return;
        }
        
        int nextIndex = Mathf.Min(_phaseIndex + 1, phases.Length - 1);
        if (nextIndex != _phaseIndex)
            SwitchPhase(_phaseIndex, nextIndex);

        EnableShieldAfterDelay();
    }

    private void EndFight()
    {
        SetShieldActive(false);
        SetVulnerable(false);

        if (heartObject) Destroy(heartObject);
        if (exitZone) exitZone.SetActive(true);
    }

    private void SetVulnerable(bool v)
    {
        if (animator) animator.SetBool(IsVulnerable, v);
    }

    private void SwitchPhase(int fromIndex, int toIndex)
    {
        if (fromIndex >= 0 && fromIndex < phases.Length && phases[fromIndex])
            phases[fromIndex].SetActive(false);

        if (toIndex >= 0 && toIndex < phases.Length && phases[toIndex])
            phases[toIndex].SetActive(true);

        _phaseIndex = toIndex;
    }

    private GameObject GetCurrentPhase()
    {
        if (_phaseIndex < 0 || _phaseIndex >= phases.Length) return null;
        return phases[_phaseIndex];
    }

    private bool AreAllDirectChildrenInactive(GameObject parent)
    {
        if (!parent) return true;

        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.activeSelf)
                return false;
        }
        return true;
    }

    private void EnableShieldAfterDelay()
    {
        if (!shield) return;

        if (_shieldRoutine != null)
            StopCoroutine(_shieldRoutine);

        _shieldRoutine = StartCoroutine(EnableShieldRoutine());
    }

    private IEnumerator EnableShieldRoutine()
    {
        SetShieldActive(false);

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(shieldActivateDelay);

        SetShieldActive(true);
    }

    private void SetShieldActive(bool active)
    {
        if (!shield) return;

        shield.SetActive(active);

        if (!_shieldCollider)
            _shieldCollider = shield.GetComponent<Collider2D>();
        
        if (_shieldCollider)
            _shieldCollider.enabled = active;
    }

    private void EjectPlayersFromHeart2D()
    {
        if (!heartCenter) return;

        Vector2 heartPos = heartCenter.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(heartPos, ejectRadius, playerMask);

        foreach (var h in hits)
        {
            if (!h) continue;

            Rigidbody2D rb = h.attachedRigidbody;
            if (!rb) continue;

            Vector2 dir = (rb.position - heartPos);
            if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;
            dir.Normalize();

            Vector2 impulse = dir * ejectOutForce + Vector2.up * ejectUpForce;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(impulse, ForceMode2D.Impulse);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!heartCenter) return;
        Gizmos.DrawWireSphere(heartCenter.position, ejectRadius);
    }
#endif
}