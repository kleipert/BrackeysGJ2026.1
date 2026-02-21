using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HeartBeat : MonoBehaviour
{
    private static readonly int IsBeating = Animator.StringToHash("IsBeating");
    [SerializeField] private Tilemap  tilemap1;
    [SerializeField] private Tilemap  tilemap2;
    [SerializeField] private float beatDuration;
    [SerializeField] private bool isActive;
    [SerializeField] private bool _isBeating;
    [SerializeField] private GameObject _player;
    [SerializeField] private AudioClip _beatStart;
    [SerializeField] private AudioClip _beatEnd;
    [SerializeField] private AudioClip _hit;
    
    private Animator _animator;
    
    void Start()
    {
        tilemap1.GameObject().SetActive(true);
        tilemap2.GameObject().SetActive(false);
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!isActive) return;
        if (!_isBeating)
        {
            _animator.SetBool(IsBeating, true);
            _isBeating = true;
            SoundManager.Instance.PlaySound(_beatStart, _player.transform, 1, 1);
        }
        
    }
    
    IEnumerator Beat()
    {
        yield return new WaitForSeconds(beatDuration);
        _isBeating = false;
    }

    public void BeatEnd()
    {
        bool on = tilemap1.GameObject().activeSelf;
        tilemap1.GameObject().SetActive(!on);
        tilemap2.GameObject().SetActive(on);
        _animator.SetBool(IsBeating, false);
        SoundManager.Instance.PlaySound(_beatEnd, _player.transform, 1,1);
        StartCoroutine(Beat());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PhaseManager.Instance.DamageHeart();
        SoundManager.Instance.PlaySound(_hit,transform,0.3f,2);
    }
}
