using System;
using UnityEngine;

namespace Misc
{
    public class PhysicalLaserBeam : MonoBehaviour
    {
        [SerializeField] private float _travelSpeed = 5f;
        private int _dir;
        private Transform _parent;

        private void Start()
        {
            _parent = transform.parent;
        }

        private void Update()
        {
            _parent.Translate(new Vector3(_dir * _travelSpeed * Time.deltaTime, 0));
        }

        public void SetDirection(int direction) => _dir = direction;

        private void OnCollisionEnter2D(Collision2D other)
        {
            // Do damage to enemies here
            Destroy(transform.parent.gameObject);
        }
    }
}
