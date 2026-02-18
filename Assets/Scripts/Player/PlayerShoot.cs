using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private GameObject _laserBeamPrefab;
        [SerializeField] private Transform _shootingPosition;

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.started)
                Instantiate(_laserBeamPrefab, _shootingPosition.position, Quaternion.identity);
        }
    }
}
