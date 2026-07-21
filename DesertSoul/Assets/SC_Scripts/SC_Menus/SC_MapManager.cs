using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class SC_MapManager : MonoBehaviour
{
    [SerializeField] GameObject[] mapPieces;

    [SerializeField] bool[] piecesActive;

    private bool hasMap = true;

    private bool hasPlayerMarker = true;

    private  SC_RoomManager room;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        room = GameObject.Find("RoomManager").GetComponent<SC_RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomDiscovered()
    {
        if(room != null)
        {
            if(room.GetHasMap() == true)
            {
                int ID = room.GetRoomID();

                piecesActive[ID] = true;
            }
            else
            {
            
            }
        }
    }
    
    public void ActivateMap()
    {
        if(hasMap)
        {
            for(int i = 0; i < mapPieces.Length; i++)
            {
                mapPieces[i].SetActive(piecesActive[i]);

                if(hasPlayerMarker)
                {
                    if(i == room.GetRoomID())
                    {
                        mapPieces[i].transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void SetHasMap(bool mapGot)
    {
        hasMap = mapGot;
    }

    public void SetPlayerMarker(bool marker)
    {
        hasPlayerMarker = marker;
    }

    public bool GetHasMap()
    {
        return hasMap;
    }

    public bool GetPlayerMarker()
    {
        return hasPlayerMarker;
    }

    public GameObject[] GetMapPieces()
    {
        return mapPieces;
    }

    public bool[] GetMapActive()
    {
        return piecesActive;
    }

    public void SetMapPieces(GameObject[] map, bool[] active)
    {
        piecesActive = active;
    }
}
