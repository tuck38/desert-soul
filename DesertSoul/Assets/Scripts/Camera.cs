using UnityEngine;

public class Camera : MonoBehaviour
{


    [SerializeField] Transform player;
    [SerializeField] Vector3 camOffset;


    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + camOffset;
    }
}
