/*using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SC_MiniMap : MonoBehaviour
{
    [SerializeField] private List<SC_MinimapRoom> rooms;
    [SerializeField] private RectTransform playerIcon;
    private Dictionary<string, SC_MinimapRoom> roomData;

    private void Awake()
    {
        ServiceLocator.Register<SC_MiniMap>(this);

        roomData = new Dictionary<string, SC_MinimapRoom>();
        foreach(SC_MinimapRoom room in rooms)
        {
            /*room.SetActive(false);

            RectTransform roomT = room.GetComponent<RectTransform>();
            roomData.Add(room.name, room);
        }
        playerIcon.gameObject.SetActive(false);
    }

    private void VisitRoom(string roomName)
    {
        if(!roomData.GetTryValue(roomName, out var room))
        {
            Devug.logWarning("No minimpa found for room " + roomName);
            return;
        }

        if(room.State == RoomState.Mapped)
        {
            room.State(RoomState.Visited);
        }
        playerIcon.GameObject.SetActive(true);
        playerIcon.localPosition = room.transform.localPosition;
    }

    public void MapRooms(List<string> roomNames)
    {
        foreach (string name in roomNames)
        {
            if(roomData.TryGetValue(name, out var room)) continue;
            room.SetState(RoomState.Mapped);
        }
    }

    public void RevealRoom(string roomName)
    {
        if(!roomPositions.TryGetValue(roomName, out var roomT))
        {
            Debug.logWarning("No room icon for room "+ roomName);
            return;
        }
        toomT.gameObject.SetActive(true);

        playerIcon.gameObject.SetActive(true);
        playerIcon.localPosition = roomT.localPosition;
    }
}*/
