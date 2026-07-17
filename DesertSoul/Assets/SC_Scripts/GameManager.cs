using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.Rendering;

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

    private UnityEngine.UI.Image fade;

    [SerializeField] float fadeTime;

    float currentFadeTime = 0;

    static float dayTimer;
    static TimeOfDay currentTime;

    [SerializeField] AudioClip town;
    [SerializeField] AudioClip mainmenu;
    [SerializeField] AudioClip area1;
    [SerializeField] AudioClip area2;
    [SerializeField] AudioClip area3;
    [SerializeField] AudioClip area4;

    [SerializeField] AudioSource musicbox;

    private bool wait = false;

    float waitTime = .2f;

    float currentWaitTime = 0f;

    SC_Areas_Enum currentArea = SC_Areas_Enum.mainmenu;

    public static GameManager Instance {  get; private set; }

    private GameObject player;

    private SC_Player_Move move;

    SC_Enum_Doors newDoor;

    bool isDoor = false;
    bool isFastTravel = false;
    bool interactableDoor = false;
    String buildingName = "Shack";

    List<string> disabledDialogueTriggers = new List<string>();
   
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

        if(wait)
        {
            if(waitTime >= currentWaitTime)
            {
                currentWaitTime += Time.deltaTime;
            }
            else
            {
                wait = false;
                move.FadeIn();
                currentWaitTime = 0;
            }
        }

        //Debug.Log($"Current Time: {dayTimer}, Time Of Day: {currentTime}");
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
    
    public void fadeIn()
    {
        if(move == null)
        {
            move = player.GetComponent<SC_Player_Move>();
        }   

        wait = true;
    }

    public void fadeOut()
    {
        if(move == null)
        {
            move = player.GetComponent<SC_Player_Move>();
        }
        move.FadeOut();
    }

    //called by player at start of scene right now, will be called by a scene manager in the future
    public void newScene()
    {
        player = GameObject.Find("Player");

        move = player.GetComponent<SC_Player_Move>();

        fade = move.getFade();

        if(isDoor)
        {
        List<SC_Door> doors = new List<SC_Door>(FindObjectsByType<SC_Door>(FindObjectsSortMode.None));
        foreach (SC_Door door in doors)
        {
            if (door.doorDir == newDoor)
            {
                door.SetDoorActive(false);
                player.transform.position = door.spawn.transform.position;
                
                fadeIn();
                break;
            }
        }
        }
        else if (interactableDoor)
        {
            //need to check if not in town for later
            List<SC_Building> builds = new List<SC_Building>(FindObjectsByType<SC_Building>(FindObjectsSortMode.None));
            foreach (SC_Building build in builds)
            {
                if(build.BuildingName == buildingName)
                {
                    player.transform.position = build.spawn.transform.position;
                    fadeIn();
                    break;
                }
            }
        }
        else if(isFastTravel)
        {
            SC_FastTravel fastTravel = FindAnyObjectByType<SC_FastTravel>();
            player.transform.position = fastTravel.transform.position;
            fadeIn();
        }

        disableDialogueTriggers();
    }

    //This function is duplicated for all different travel methods
    //In Order: directional doors, interactable doors, fast travel

    //directional doors 
    public void LoadNewLevel(string scene, SC_Areas_Enum area, SC_Enum_Doors doorDir, bool door, bool fastTravel, bool intDoor = false, string toBuildingName = "Shack")
    {
        //Gets the source door direction and finds the destination door in the next room
        //I hate this one

        isDoor = door;
        interactableDoor = intDoor;
        isFastTravel = fastTravel;
        buildingName = toBuildingName;

        if(door)
        {
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
        }
        //this is like ACTUALLY painful to look at
        //Toby Fox core
        //I majored in Game Programming


        player = GameObject.Find("Player");

        move = player.GetComponent<SC_Player_Move>();

        fade = move.getFade();
        NewArea(area);
        SceneManager.LoadScene(scene);
    }


    public void playBossTheme(AudioClip bossMusic)
    {
        musicbox.clip = bossMusic;
        musicbox.Play();
    }

    //not finished function
    private void NewArea(SC_Areas_Enum area)
    {
        if(area != currentArea)
        {
            switch (area)
            {
                case SC_Areas_Enum.town:
                    musicbox.clip = town;
                    musicbox.Play();
                    currentArea = area;
                    break;

                case SC_Areas_Enum.mainmenu:
                    musicbox.clip = mainmenu;
                    musicbox.Play();
                    currentArea = area;
                    break;

                case SC_Areas_Enum.hotdry:
                    musicbox.clip = area1;
                    musicbox.Play();
                    currentArea = area;
                    break;

                case SC_Areas_Enum.arid:
                    musicbox.clip = area2;
                    musicbox.Play();
                    currentArea = area;
                    break;

                case SC_Areas_Enum.costal:
                    musicbox.clip = area3;
                    musicbox.Play();
                    currentArea = area;
                    break;

                case SC_Areas_Enum.frozen:
                    musicbox.clip = area4;
                    musicbox.Play();
                    currentArea = area;
                    break;

                case SC_Areas_Enum.boss:
                    musicbox.Stop();
                    currentArea = area;
                    break;
            }
        }
    }

    private void disableDialogueTriggers()
    {
        GameObject[] triggers = GameObject.FindGameObjectsWithTag("DialogueTrigger");
        Debug.Log(disabledDialogueTriggers);
        foreach (GameObject trigger in triggers)
        {
            if (disabledDialogueTriggers.Contains(trigger.name))
            {
                trigger.SetActive(false);
            }
        }
    }

    public void addTriggerToDisabledList(string trigger)
    {
        if (!disabledDialogueTriggers.Contains(trigger))
        {
            disabledDialogueTriggers.Add(trigger);
        }
    }
}
