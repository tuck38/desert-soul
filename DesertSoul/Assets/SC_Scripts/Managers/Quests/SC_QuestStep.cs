using Unity.VisualScripting;
using UnityEngine;

public class SC_QuestStep 
{
    [SerializeField] string Description;

    [SerializeField] Enum_QuestStepType stepType;

    //IF COLLECT QUEST
    [SerializeField] ResouceTypes typeToCollect;

    [SerializeField] int amountToCollect;

    private int amountCollected = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmountCol(int col)
    {
        amountCollected = col;
    }

    public int GetAmountCol()
    {
        return amountCollected;
    }

    

}
