using UnityEngine;

public class SC_RoomManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private int roomID;

    [SerializeField] private AudioClip roomMusic;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetRoomID()
    {
        return roomID;
    }
}
