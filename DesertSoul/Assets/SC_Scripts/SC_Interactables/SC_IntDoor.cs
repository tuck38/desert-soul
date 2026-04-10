using UnityEngine;

public class SC_IntDoor : SC_Interactable
{

    [SerializeField] string building = "Shack";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void DoAction()
    {
        GameManager.Instance.LoadNewLevel(goToScene, SC_Enum_Doors.Bottom, false, false, true, building);
    }
}
