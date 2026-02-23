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
            SoundManager.Instance.PlaySound(thunder, transform, 1f);
            _anim.SetInteger(LightningMode, _mode);
        }

        public void SetLightningMode(int mode)
        {
            _mode = mode;
        }

        private void AnimationDone()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var xPos = Mathf.Abs(player.transform.position.x - transform.position.x);
            var yPos = Mathf.Abs(player.transform.position.y - (transform.position.y + .5f));
            if(xPos <= 1f && yPos <= .5f)
                HealthManager.Instance.TakeDamage(1);
            SoundManager.Instance.PlaySound(lightning, transform, 1f);
            Destroy(gameObject);
        }
    }
}
