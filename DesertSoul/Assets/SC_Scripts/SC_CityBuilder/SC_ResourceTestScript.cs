using System;
using UnityEngine;
using TMPro;

public class SC_ResourceTestScript : MonoBehaviour
{
    public static Action<int, int> OnResourcesAmountChanged;

    public static int amountOfBrown { get; private set; }
    public static int amountOfPurple { get; private set; }

    [SerializeField] TextMeshProUGUI brownText;
    [SerializeField] TextMeshProUGUI purpleText;

    void OnEnable()
    {
        OnResourcesAmountChanged += UpdateResources;
        UpdateResources(1000, 1000);
    }

    void OnDisable()
    {
        OnResourcesAmountChanged -= UpdateResources;
    }

    /// <summary>
    /// Update the total amount of resources available to spend based on input
    /// </summary>
    /// <param name="numBrownSpent"></param>
    /// <param name="numPurpleSpent"></param>
    void UpdateResources(int numBrownSpent, int numPurpleSpent)
    {
        amountOfBrown += numBrownSpent;
        amountOfPurple += numPurpleSpent;

        brownText.text = amountOfBrown.ToString();
        purpleText.text = amountOfPurple.ToString();
    }

}
