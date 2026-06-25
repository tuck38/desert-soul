using UnityEngine;
using Yarn.Unity;

public class SC_DialogueManager : MonoBehaviour
{

    public GameObject player;
    public GameObject cameraFollow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerMovement(bool canMove)
    {
        player.GetComponent<SC_Player_Move>().SetCanMove(canMove);
    }

    [YarnCommand("change_camera_target")]
    public void SetCameraFollow(GameObject target)
    {
        cameraFollow.GetComponent<SC_Camera_FollowObject>().DialogueTarget(target);
    }

    [YarnCommand("flip_target")]
    public void FlipTargetSprite(GameObject target, bool flip)
    {
        target.GetComponent<SpriteRenderer>().flipX = flip;
    }
}
