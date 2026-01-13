using Unity.VisualScripting;
using UnityEngine;

public class SC_Door : MonoBehaviour
{
    BoxCollider2D doorTrigger;
    [SerializeField] public SC_Enum_Doors doorDir;
    [SerializeField] string nextScene;
    [SerializeField] public GameObject spawn;
    private bool movePlayer;
    Vector2 movement;

    //Completely works for horizontal doors rn, does not for up/down. can do some band aid solutions to get it working but

    private bool doorActive = true;

    [SerializeField] float doorSpeed = 4f;

    SC_Player_Move playerMove;

    private void Awake()
    {
        doorTrigger = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        playerMove = GameObject.Find("Player").GetComponent<SC_Player_Move>();
    }

    private void Update()
    {
        if(movePlayer)
        {
            //unaware if this is more expensive then just getting the component here
            playerMove.SetCanMove(false);

            //if it is a top or bottom door(zamn!) tell the player movefunction to not use gravity
            if (doorDir == SC_Enum_Doors.Top || doorDir == SC_Enum_Doors.Top2 || doorDir == SC_Enum_Doors.Bottom || doorDir == SC_Enum_Doors.Bottom2)
            {
                playerMove.Move(movement, doorSpeed, false);
            }
            else
            {
                playerMove.Move(movement, doorSpeed, true);
            }
        }
    }

    public void SetDoorActive(bool isActive)
    {
        this.doorActive = isActive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            //set movement to the direction of the door, so the player will travel into it
            if (doorActive)
            {
                if (doorDir == SC_Enum_Doors.Left || doorDir == SC_Enum_Doors.Left2)
                {
                    movement = new Vector2(-1, 0);
                }
                else if (doorDir == SC_Enum_Doors.Right || doorDir == SC_Enum_Doors.Right2)
                {
                    movement = new Vector2(1, 0);
                }
                else if (doorDir == SC_Enum_Doors.Top || doorDir == SC_Enum_Doors.Top2)
                {
                    movement = new Vector2(0, 1);
                }
                else if (doorDir == SC_Enum_Doors.Bottom || doorDir == SC_Enum_Doors.Bottom2)
                {
                    movement = new Vector2(0, -1);
                }
            }
            //set movement to the direction opposite the door, so the player will travel out of it
            else if (!doorActive)
            {
                if (doorDir == SC_Enum_Doors.Left || doorDir == SC_Enum_Doors.Left2)
                {
                    movement = new Vector2(1, 0);
                }
                else if (doorDir == SC_Enum_Doors.Right || doorDir == SC_Enum_Doors.Right2)
                {
                    movement = new Vector2(-1, 0);
                }
                else if (doorDir == SC_Enum_Doors.Top || doorDir == SC_Enum_Doors.Top2)
                {
                    movement = new Vector2(0, -1);
                }
                else if (doorDir == SC_Enum_Doors.Bottom || doorDir == SC_Enum_Doors.Bottom2)
                {
                    movement = new Vector2(0, 1);
                }
            }

            movePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Player moving into a door far enough to load the new scene
            if (doorActive)
            {
                movePlayer = false;
                if (collision.gameObject.tag == "Player")
                {
                    GameManager.Instance.LoadNewLevel(nextScene, doorDir, true, false);
                }
            }
            //Player moving out from a door into a new room
            else if (!doorActive)
            {
                movePlayer = false;
                playerMove.SetCanMove(true);
                doorActive = true;
            }
        }
    }
}
