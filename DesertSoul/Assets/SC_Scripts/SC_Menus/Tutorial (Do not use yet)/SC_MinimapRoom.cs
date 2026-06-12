/*using UnityEngine;

public class SC_MinimapRoom : MonoBehaviour
{
    public RoomState State {get;private set;}
    [SerializeField] private RectTransform rect;
    [SerializeField] private GameObject mappedRoom;
    [SerializeField] private GameObject visitedRoom;

    private void Awake()
    {
        SetState(State);
    }

    public void SetState(RoomState newState)
    {
        State =  newState;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        switch (State)
        {
            case RoomState.Hidden: mappedRoom.SetActive(false);
            visitedRoom.SetActive(false);
            break;

            case RoomState.Mapped: mappedRoom.SetActive(true);
            visitedRoom.SetActive(false);
            break;

            case RoomState.Visited:visitedRoom.SetActive(true);
            break;
        }
    }

    public enum RoomState
    {
        Hidden,
        Mapped,
        Visited
    }
}*/
