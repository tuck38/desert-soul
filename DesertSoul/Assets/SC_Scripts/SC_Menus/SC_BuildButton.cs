using UnityEngine;

public class SC_BuildButton : MonoBehaviour
{

    [SerializeField] SC_Building building;

    [SerializeField] GameObject X;

    [SerializeField] public string buildName;

    public bool IsActive;

    public int BuildID;

    public bool disabled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string GetBuildName()
    {
        return building.name;
    }

    Vector2 GetMats()
    {
        return building.MaterialCost;
    }

    public void DISABLE()
    {
        disabled = true;
        X.SetActive(true);
    }
}
