using UnityEngine;

public class SC_MapManager : MonoBehaviour
{
    [SerializeField] GameObject[] mapPieces;

    [SerializeField] bool[] piecesActive;

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
        if(room.GetHasMap() == true)
        {
            Debug.Log("map");

            int ID = room.GetRoomID();

            piecesActive[ID] = true;
        }
        else
        {
            Debug.Log("no map");
        }
    }
    
    public void MapActivate()
    {
        for(int i = 0; i < mapPieces.Length; i++)
        {
            mapPieces[i].SetActive(piecesActive[i]);
        }
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
        Debug.Log("setting");
        piecesActive = active;
    }
}
