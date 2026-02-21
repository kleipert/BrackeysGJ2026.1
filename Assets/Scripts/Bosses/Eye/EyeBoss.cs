using System.Collections;
using UnityEngine;

public class EyeBoss : MonoBehaviour
{
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int IsVulnerable = Animator.StringToHash("IsVulnerable");
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject laser;
    [SerializeField] private float turnSpeed;
    [SerializeField] private bool _isActive;
    [Header("Sound")]
    [SerializeField] private AudioClip _laserBeam;
    [SerializeField] private AudioClip _laserLoad;
    
    private bool _inAttack;
    private Animator _animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player || !_isActive) return;

        Vector3 dirc = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dirc.y, dirc.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, turnSpeed * Time.deltaTime);

        if (_inAttack) return;
        
        _inAttack  = true;
        StartCoroutine(StartAttack());

    }

    public void ActivateBoss()
    {
        _isActive = true;
    }

    IEnumerator StartAttack()
    {
        SoundManager.Instance.PlaySound(_laserLoad,transform,1f,5);
        yield return new WaitForSeconds(5f);
        laser.SetActive(true);
        _animator.SetBool(IsAttacking, true);

        SoundManager.Instance.PlaySound(_laserBeam,transform,0.3f,5);
        yield return new WaitForSecondsRealtime(10f);
        _animator.SetBool(IsAttacking, false);
        _animator.SetBool(IsVulnerable, true);
        laser.SetActive(false);

        yield return new WaitForSeconds(10f);
        _animator.SetBool(IsVulnerable, false);
        _inAttack = false;
    }

}

