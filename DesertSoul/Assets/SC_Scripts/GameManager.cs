using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public enum TimeOfDay
{
    MORNING,
    MIDDAY,
    NIGHT
}

public class GameManager : MonoBehaviour
{
    public static Action<TimeOfDay> OnTimeOfDayChanged;

    [Header("Day/Night System Variables")]
    [SerializeField] float dayNightClockSpeed = 1f;
    [SerializeField] float minTime = 0f;
    [SerializeField] float maxTime = 24f;
    [Tooltip("Indexes in this array correspond to the indexes attached to the values in the TimeOfDay enum")]
    [SerializeField] float[] timeOfDayStartTimes;

    static float dayTimer;
    static TimeOfDay currentTime;

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

    private void Update()
    {
        dayTimer += Time.deltaTime * dayNightClockSpeed;
        if (dayTimer >= maxTime) dayTimer -= maxTime;
        UpdateTimeOfDay();
        Debug.Log($"Current Time: {dayTimer}, Time Of Day: {currentTime}");
    }

    private void UpdateTimeOfDay()
    {
        TimeOfDay newTime = currentTime;

        if (dayTimer >= timeOfDayStartTimes[(int)TimeOfDay.NIGHT]) newTime = TimeOfDay.NIGHT;
        else if (dayTimer >= timeOfDayStartTimes[(int)TimeOfDay.MIDDAY]) newTime = TimeOfDay.MIDDAY;
        else if (dayTimer >= timeOfDayStartTimes[(int)TimeOfDay.MORNING]) newTime = TimeOfDay.MORNING;

        if (newTime != currentTime)
        {
            currentTime = newTime;
            OnTimeOfDayChanged?.Invoke(currentTime);
        }
    }

    //called by player at start of scene right now, will be called by a scene manager in the future
    public void newScene()
    {
        player = GameObject.Find("Player");
        List<SC_Door> doors = new List<SC_Door>(FindObjectsByType<SC_Door>(FindObjectsSortMode.None));
        foreach (SC_Door door in doors)
        {
            if (door.doorDir == newDoor)
            {
                door.SetDoorActive(false);
                player.transform.position = door.spawn.transform.position;
                break;
            }
        }
    }

    public void LoadNewLevel(string scene, SC_Enum_Doors doorDir)
    {
        //Gets the source door direction and finds the destination door in the next room
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
