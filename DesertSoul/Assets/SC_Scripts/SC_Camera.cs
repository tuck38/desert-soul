using Unity.Cinemachine;
using UnityEngine;

public class SC_Camera : MonoBehaviour
{
/*This Script is for the camera to follow the player with sprite rotation, most likely will be deprecated when we figure 
out the new cinemachine*/
    [SerializeField] Transform player;
    [SerializeField] Vector3 camOffset;
    [SerializeField] CinemachineCamera cinemachineCamera;
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

    /// <summary>
    /// Switches camera to shop view
    /// </summary>
    /// <param name="newTarget"></param>
    /// <param name="cameraZoom"></param>
    public void SetShopView(Transform newTarget, float cameraZoom)
    {
        cinemachineCamera.Lens.OrthographicSize = cameraZoom;
        activeTarget = newTarget;
    }

    /// <summary>
    /// Switches camera to player view
    /// </summary>
    public void DefaultView()
    {
        cinemachineCamera.Lens.OrthographicSize = defaultOrthographicSize;
        activeTarget = player;
    }
}
