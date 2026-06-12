using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SC_DuctTapeMiniMap : MonoBehaviour
{
    public static SC_DuctTapeMiniMap Instance;
    static bool isMapActive = false;
    public List<GameObject> maps = new List<GameObject>();
    SC_Player_HUD playerHud;
    
    

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //Objective is to get the interger value from sc_player hud, the system checks the 
        //number as the assigned map image then sets it to be active at the start of the scene
        //playerHud.mapPart = [number in map list].SetActive(true);
    }
}
