using UnityEngine;

public class BloodDoorVessel : MonoBehaviour
{
    private void OnDestroy() => GetComponentInParent<BloodDoor>().RemoveVessel();
}
