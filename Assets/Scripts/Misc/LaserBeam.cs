using UnityEngine;

namespace Misc
{
    public class LaserBeam : MonoBehaviour
    {
        [SerializeField] private GameObject physical_beam;
        
        public void ActivateBeam()
        {
            var facingDir = GameObject.Find("Player").GetComponent<PlayerMovement>().FacingDirection;
            transform.localScale = new Vector3(transform.localScale.x * facingDir, transform.localScale.y,
                transform.localScale.z);
            physical_beam.GetComponent<PhysicalLaserBeam>().SetDirection(facingDir);
            physical_beam.SetActive(true); 
            
        }
    }
}
