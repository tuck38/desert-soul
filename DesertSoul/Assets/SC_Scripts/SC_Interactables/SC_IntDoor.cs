using UnityEngine;

public class SC_IntDoor : SC_Interactable
{

    [SerializeField] string building = "Shack";

    [SerializeField] SC_Areas_Enum area = SC_Areas_Enum.town;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void DoAction()
    {
        GameManager.Instance.LoadNewLevel(goToScene, area, SC_Enum_Doors.Bottom, false, false, true, building);
    }
}
