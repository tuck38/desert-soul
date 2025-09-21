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
            playerMove.Move(movement, doorSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        switch (doorDir)
        {
            case SC_Enum_Doors.Left:

                movement = new Vector2(-1, 0);
                break;

            case SC_Enum_Doors.Left2:

                movement = new Vector2(-1, 0);
                break;

            case SC_Enum_Doors.Right:

                movement = new Vector2(1, 0);
                break;

            case SC_Enum_Doors.Right2:

                movement = new Vector2(1, 0);
                break;

            case SC_Enum_Doors.Top:

                //i will deal with you later
                break;

            case SC_Enum_Doors.Top2:

                //i will deal with you later
                break;

            case SC_Enum_Doors.Bottom:

                //i will deal with you later
                break;

            case SC_Enum_Doors.Bottom2:

                //i will deal with you later
                break;

            default:
                Debug.Log("RAHHHH");
                break;
        }
        movePlayer = true;
        //GameManager.Instance.LoadNewLevel(nextScene, doorDir);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        movePlayer = false;
        playerMove.Move(movement, doorSpeed);
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.LoadNewLevel(nextScene, doorDir);
        }
    }
}
