using System;
using System.Collections;
using Bosses;
using UnityEngine;

public class EyeBoss : MonoBehaviour
{
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int IsVulnerable = Animator.StringToHash("IsVulnerable");

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject laser;
    [SerializeField] private float turnSpeed;
    [SerializeField] private bool _isActive;

    [Header("Rotation")]
    [SerializeField] private float vulnerableRotationZ = 90f;

    [Header("Sound")]
    [SerializeField] private AudioSource _laserBeam;
    [SerializeField] private AudioClip _laserLoad;
    [SerializeField] private AudioClip _bossHit;

    [SerializeField] private GameObject _levelExitZone;
    [SerializeField] private GameObject _headZone;
    private BossHealth _bossHealth;

    private bool _inAttack;
    private bool _lockRotationToVulnerableAngle;
    private Animator _animator;
    
    private Coroutine _attackRoutine;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _bossHealth = _headZone.GetComponent<BossHealth>();
        _bossHealth.onDamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken(object sender, EventArgs e)
    {
        _headZone.SetActive(false);
        SoundManager.Instance.PlaySound(_bossHit, transform, 1f);

        if (_bossHealth.GetCurrentHealth() == 0)
        {
            Destroy(gameObject);
            return;
        }
        
        RestartAttackImmediately();
    }

    private void RestartAttackImmediately()
    {
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
        }
        
        laser.SetActive(false);
        if (_laserBeam.isPlaying) _laserBeam.Stop();

        _lockRotationToVulnerableAngle = false;
        _animator.SetBool(IsVulnerable, false);
        _animator.SetBool(IsAttacking, false);
        _headZone.SetActive(false);
        
        _inAttack = true;
        _attackRoutine = StartCoroutine(StartAttack());
    }

    void Update()
    {
        if (!player || !_isActive) return;

        float targetAngle;
        if (_lockRotationToVulnerableAngle)
        {
            targetAngle = vulnerableRotationZ;
        }
        else
        {
            Vector3 dirc = player.transform.position - transform.position;
            targetAngle = Mathf.Atan2(dirc.y, dirc.x) * Mathf.Rad2Deg;
        }

        Quaternion q = Quaternion.AngleAxis(targetAngle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, turnSpeed * Time.deltaTime);

        if (_inAttack) return;

        _inAttack = true;
        _attackRoutine = StartCoroutine(StartAttack());
    }

    public void ActivateBoss()
    {
        _isActive = true;
    }

    IEnumerator StartAttack()
    {
        SoundManager.Instance.PlaySound(_laserLoad, transform, 1f, 5);
        _animator.SetBool(IsAttacking, true);
        yield return new WaitForSeconds(5f);

        laser.SetActive(true);
        _laserBeam.Play();
        yield return new WaitForSeconds(10f);

        _animator.SetBool(IsAttacking, false);
        _animator.SetBool(IsVulnerable, true);
        _headZone.SetActive(true);
        laser.SetActive(false);
        _laserBeam.Stop();

        _lockRotationToVulnerableAngle = true;
        yield return new WaitForSecondsRealtime(15f);

        _lockRotationToVulnerableAngle = false;
        _animator.SetBool(IsVulnerable, false);
        _headZone.SetActive(false);

        _inAttack = false;
        _attackRoutine = null;
    }

    private void OnDestroy()
    {
        if (_bossHealth != null)
            _bossHealth.onDamageTaken -= OnDamageTaken;

        if (_levelExitZone != null)
            _levelExitZone.SetActive(true);

        _levelExitZone = null;
        _bossHealth = null;
    }
}