using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SC_PlayerData
{
    public int level;
    public int playerHP;
    public float[] location;

    public SC_PlayerData (SC_Player_Prop player)
    {
        //level = player.level;
        //playerHP = player.currentHealthProxy;

        location = new float[3];
        location[0] = player.transform.position.x;
        location[1] = player.transform.position.y;
        location[2] = player.transform.position.z;
    }

}
