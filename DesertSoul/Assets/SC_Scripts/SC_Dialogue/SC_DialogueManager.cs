using UnityEngine;
using Yarn.Unity;

public class SC_DialogueManager : MonoBehaviour
{

    public GameObject player;
    public GameObject cameraFollow;
    public GameObject[] dialogueUI;

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

    [YarnCommand("move_target_to_position")]
    public void MoveTargetToPosition(GameObject target, float x, float y)
    {
        Vector2 position = new Vector2(x, y); 
        target.GetComponent<SC_NPC_Movement>().movementTarget = position;
    }

    [YarnCommand("move_player_to_position")]
    public void MovePlayerToPosition(float x, float y)
    {
        Vector2 position = new Vector2(x, y); 
        player.GetComponent<SC_Player_Move>().Move(position, 2f, true);
    }

    [YarnCommand("disable_player_ui")]
    public void DisablePlayerUI(bool isDisabled)
    {
        foreach (GameObject ui in dialogueUI)
        {
            ui.SetActive(!isDisabled);
        }
    }

    [YarnCommand("disable_dialogue_trigger")]
    public void DisableDialogueTrigger(GameObject trigger)
    {
        GameManager.Instance.GetComponent<GameManager>().addTriggerToDisabledList(trigger.name);
    }
}
