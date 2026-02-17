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
            {
                var worldPos = _shootingPosition.TransformPoint(Vector3.zero);
                Instantiate(_laserBeamPrefab, _shootingPosition.position, Quaternion.identity);
            }
        }
    }
}
