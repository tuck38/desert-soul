using System;
using UnityEngine;

public class SC_BuildingDoor : MonoBehaviour
{
    [SerializeField] SC_Building building;

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
        building.ToggleBuildingActive();
    }
}
