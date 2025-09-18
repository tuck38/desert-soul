using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance {  get; private set; }

    private GameObject player;

    SC_Enum_Doors newDoor;
   
    private void Awake()
    {
        //singleton method
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //called by player at start of scene right now, will be called by a scene manager in the future
    public void newScene()
    {
        player = GameObject.Find("Player");

        List<SC_Door> doors = new List<SC_Door>(FindObjectsByType<SC_Door>(FindObjectsSortMode.None));
        foreach (SC_Door door in doors)
        {
            //Debug.Log(newDoor);
            //Debug.Log(door.doorDir);
            player.transform.position = door.spawn.transform.position;
            if (door.doorDir == newDoor)
            {
                player.transform.position = door.spawn.transform.position;
                break;
            }
        }
    }

    public void LoadNewLevel(string scene, SC_Enum_Doors doorDir)
    {

        //I hate this one
        switch (doorDir)
        {
            case SC_Enum_Doors.Left:

                newDoor = SC_Enum_Doors.Right;
                break;

            case SC_Enum_Doors.Left2:

                newDoor = SC_Enum_Doors.Right2;
                break;

            case SC_Enum_Doors.Right:

                newDoor = SC_Enum_Doors.Left;
                break;

            case SC_Enum_Doors.Right2:

                newDoor = SC_Enum_Doors.Left2;
                break;

            case SC_Enum_Doors.Top:

                newDoor = SC_Enum_Doors.Bottom;
                break;

            case SC_Enum_Doors.Top2:

                newDoor = SC_Enum_Doors.Bottom2;
                break;

            case SC_Enum_Doors.Bottom:

                newDoor = SC_Enum_Doors.Top;
                break;

            case SC_Enum_Doors.Bottom2:

                newDoor = SC_Enum_Doors.Top2;
                break;

            default:
                newDoor = SC_Enum_Doors.Left;
                break;
        }
        //this is like ACTUALLY painful to look at
        //Toby Fox core
        //I majored in Game Programming

        SceneManager.LoadScene(scene);
    }
}
