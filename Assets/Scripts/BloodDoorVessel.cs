using UnityEngine;

public class BloodDoorVessel : MonoBehaviour
{
    private void OnDestroy()
    {
        var door = GetComponentInParent<BloodDoor>();
        if(door)
            door.RemoveVessel();
    } 
}
