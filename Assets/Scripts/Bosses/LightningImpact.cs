using System;
using UnityEngine;

namespace Bosses
{
    public class LightningImpact : MonoBehaviour
    {
        private Animator _anim;
        private int _mode = 0;
        private static readonly int LightningMode = Animator.StringToHash("lightningMode");

        private void Start()
        {
            _anim = GetComponent<Animator>();
            _anim.SetInteger(LightningMode, _mode);
        }

        public void SetLightningMode(int mode)
        {
            _mode = mode;
        }

        private void AnimationDone()
        {
            // Damage player
            Destroy(gameObject);
        }
    }
}
