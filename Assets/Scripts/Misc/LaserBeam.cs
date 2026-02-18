using UnityEngine;

namespace Misc
{
    public class LaserBeam : MonoBehaviour
    {
        [SerializeField] private GameObject physical_beam;
        private int _facingDirection;

        private void Start()
        {
            physical_beam.SetActive(false);
            _facingDirection = GameManager.GameManagerSingleton.Instance.GetPlayerFacingDirection();
            transform.localScale = new Vector3(transform.localScale.x * _facingDirection, transform.localScale.y,
                transform.localScale.z);
        }

        public void ActivateBeam()
        {
            physical_beam.GetComponent<PhysicalLaserBeam>().SetDirection(_facingDirection);
            physical_beam.SetActive(true); 
        }
    }
}
