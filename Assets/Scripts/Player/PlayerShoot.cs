using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private GameObject _laserBeamPrefab;
        [SerializeField] private Transform _shootingPosition;
        [SerializeField] private AudioClip _audioClip;

        private bool _cooldown;

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.started && !_cooldown)
            {
                Instantiate(_laserBeamPrefab, _shootingPosition.position, Quaternion.identity);
                SoundManager.Instance.PlaySound(_audioClip, transform, 0.3f);
                _cooldown = true;
                StartCoroutine(Cooldown());
            }
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(1f);
            _cooldown = false;
        }
    }
}
