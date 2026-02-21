using System.Collections.Generic;
using UnityEngine;

public class BloodDoor : MonoBehaviour
{
    [SerializeField] private int _vesselsLeft;
    [SerializeField] private GameObject _tilemapToOpen;
    

    // Update is called once per frame
    void Update()
    {
        if (_vesselsLeft == 0)
        {
            _tilemapToOpen.SetActive(false);
        }
    }

    public void RemoveVessel() => _vesselsLeft--;
}
