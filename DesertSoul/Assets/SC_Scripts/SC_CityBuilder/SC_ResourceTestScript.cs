using System;
using UnityEngine;
using TMPro;

public enum ResouceTypes
{
    STONE,
    TWINE,
    FRUIT,
    ICE
}

public class SC_ResourceTestScript : MonoBehaviour
{
    public static int amountOfType1 { get; private set; }
    public static int amountOfType2 { get; private set; }
    public static int amountOfType3 { get; private set; }
    public static int amountOfType4 { get; private set; }

    [SerializeField] TextMeshProUGUI type1Text;
    [SerializeField] TextMeshProUGUI type2Text;
    [SerializeField] TextMeshProUGUI type3Text;
    [SerializeField] TextMeshProUGUI type4Text;

    static bool runOnce = false;

    private void Awake()
    {
        Debug.Log("Grabbing UI");
        GameObject ui = GameObject.FindGameObjectWithTag("ResourceUI");
        type1Text = ui.transform.GetChild(0)?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
        type2Text = ui.transform.GetChild(1)?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
        type3Text = ui.transform.GetChild(2)?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
        type4Text = ui.transform.GetChild(3)?.GetChild(0)?.GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        //OnResourcesAmountChanged += UpdateResources;
        /*if(!runOnce)
        {
            UpdateResources(ResouceTypes.STONE, 1000);
            UpdateResources(ResouceTypes.TWINE, 900);
            UpdateResources(ResouceTypes.FRUIT, 50);
            UpdateResources(ResouceTypes.ICE, 10);
            runOnce = true;
        }
        else
        {
            UpdateResources(ResouceTypes.STONE, 0);
            UpdateResources(ResouceTypes.TWINE, 0);
            UpdateResources(ResouceTypes.FRUIT, 0);
            UpdateResources(ResouceTypes.ICE, 0);
        }*/
    }

    void OnDisable()
    {
        //OnResourcesAmountChanged -= UpdateResources;
    }

    /// <summary>
    /// Update the total amount of resources available to spend based on input
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="amountOfResouceChanged"></param>
    void UpdateResources(ResouceTypes resourceType, int amountOfResouceChanged)
    {
        switch(resourceType)
        {
            case ResouceTypes.STONE:
                amountOfType1 += amountOfResouceChanged;
                if(type1Text) type1Text.text = amountOfType1.ToString();
                break;
            case ResouceTypes.TWINE:
                amountOfType2 += amountOfResouceChanged;
                if(type2Text) type2Text.text = amountOfType2.ToString();
                break;
            case ResouceTypes.FRUIT:
                amountOfType3 += amountOfResouceChanged;
                if (type3Text) type3Text.text = amountOfType3.ToString();
                break;
            case ResouceTypes.ICE:
                amountOfType4 += amountOfResouceChanged;
                if (type4Text) type4Text.text = amountOfType4.ToString();
                break;
        }
    }

}
