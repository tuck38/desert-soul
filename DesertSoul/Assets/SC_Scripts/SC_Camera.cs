using UnityEngine;

public class SC_Camera : MonoBehaviour
{
/*This Script is for the camera to follow the player with sprite rotation, most likely will be deprecated when we figure 
out the new cinemachine*/
    [SerializeField] Transform player;
    [SerializeField] Vector3 camOffset;


    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + camOffset;
    }
}
