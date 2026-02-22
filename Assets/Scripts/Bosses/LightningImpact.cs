using System;
using UnityEngine;

namespace Bosses
{
    public class LightningImpact : MonoBehaviour
    {
        [Header("Sound")] 
        [SerializeField] private AudioClip thunder;
        [SerializeField] private AudioClip lightning;

        [SerializeField] private LayerMask _whatIsPlayer;
        
        private Animator _anim;
        private int _mode = 0;
        private static readonly int LightningMode = Animator.StringToHash("lightningMode");

        private void Start()
        {
            _anim = GetComponent<Animator>();
            SoundManager.Instance.PlaySound(thunder, transform, 0.3f);
            _anim.SetInteger(LightningMode, _mode);
        }

        public void SetLightningMode(int mode)
        {
            _mode = mode;
        }

        private void AnimationDone()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if(Mathf.Abs(player.transform.position.x - transform.position.x) <= 1f)
                HealthManager.Instance.TakeDamage(1);
            SoundManager.Instance.PlaySound(lightning, transform, 0.3f);
            Destroy(gameObject);
        }
    }
}
