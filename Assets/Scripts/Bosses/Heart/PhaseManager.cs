using System.Collections;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private static readonly int IsVurneable = Animator.StringToHash("IsVurneable");

    [SerializeField] private GameObject phase1;
    [SerializeField] private GameObject phase2;
    [SerializeField] private GameObject phase3;

    [SerializeField] private GameObject shield;

    [SerializeField] private Transform heartCenter;
    [SerializeField] private LayerMask playerMask;

    [SerializeField] private Animator animator;

    [SerializeField] private float ejectRadius = 3f;
    [SerializeField] private float ejectOutForce = 8f;
    [SerializeField] private float ejectUpForce = 6f;

    [SerializeField] private float shieldActivateDelay = 0.15f;

    [SerializeField] private int healthHeart = 3;
    [SerializeField] private GameObject _exitZone;
    [SerializeField] private GameObject _heart;

    public static PhaseManager Instance { get; private set; }

    private int _currentPhaseIndex = 0;
    private bool _waitingForHeartDamage = false;

    private Collider2D _shieldCollider;
    private Coroutine _shieldRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (shield)
            _shieldCollider = shield.GetComponent<Collider2D>();
    }

    private void Start()
    {
        phase1.SetActive(true);
        phase2.SetActive(false);
        phase3.SetActive(false);

        _currentPhaseIndex = 0;
        _waitingForHeartDamage = false;

        SetShieldActive(true);
        if (animator) animator.SetBool(IsVurneable, false);
    }

    private void Update()
    {
        if (!_waitingForHeartDamage && AreAllDirectChildrenInactive(GetCurrentPhase()))
        {
            SetShieldActive(false);
            if (animator) animator.SetBool(IsVurneable, true);
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
            
            if (animator) animator.SetBool(IsVurneable, false);
            _waitingForHeartDamage = false;
            
            EjectPlayersFromHeart2D();
            
            EnableShieldAfterDelay();
        }
        else
        {
            SetShieldActive(false);
        }

        if (_currentPhaseIndex == 2)
        {
            Destroy(_heart);
            _exitZone.SetActive(true);
        }
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(heartPos, ejectRadius, playerMask);

        foreach (var h in hits)
        {
            if (!h || h.isTrigger) continue;

            Rigidbody2D rb = h.attachedRigidbody;
            if (!rb) continue;

            Vector2 dir = (rb.position - heartPos);
            if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;
            dir = dir.normalized;

            Vector2 impulse = dir * ejectOutForce + Vector2.up * ejectUpForce;

            rb.linearVelocity = Vector2.zero; 
            rb.AddForce(impulse, ForceMode2D.Impulse);
        }
    }
}