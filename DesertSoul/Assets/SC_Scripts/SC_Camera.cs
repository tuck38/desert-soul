using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class SC_Camera : MonoBehaviour
{
/*This Script is for the camera to follow the player with sprite rotation, most likely will be deprecated when we figure 
out the new cinemachine*/
    [SerializeField] Transform player;
    [SerializeField] Vector3 camOffset;
    [SerializeField] CinemachineCamera currentCamera;
    [SerializeField] float defaultOrthographicSize;

    Transform activeTarget;

    private void Start()
    {
        activeTarget = player;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = activeTarget.position + camOffset;
    }


    //SET SHOP VIEW CHANGE TO NEW SYSTEM WHEN SET UP
    /// <summary>
    /// Switches camera to shop view
    /// </summary>
    /// <param name="newTarget"></param>
    /// <param name="cameraZoom"></param>
    public void SetShopView(Transform newTarget, float cameraZoom)
    {
        currentCamera.Lens.OrthographicSize = cameraZoom;
        activeTarget = newTarget;
    }

    /// <summary>
    /// Switches camera to player view
    /// </summary>
    public void DefaultView()
    {
        currentCamera.Lens.OrthographicSize = defaultOrthographicSize;
        activeTarget = player;
    }
    // SHOP END
}
