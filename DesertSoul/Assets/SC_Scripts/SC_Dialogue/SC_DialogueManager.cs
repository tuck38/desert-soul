using UnityEngine;
using TMPro;
using Ink.Runtime;

public class SC_DialogueManager : MonoBehaviour
{
    private static SC_DialogueManager instance;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI displayText;

    public int testItemCount = 0;
    bool sendingData = false;

    Story currentStory;
    public bool currentDialogue;

    string varNames;
    int dataVal;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple Dialogue Manager Instances");
        instance = this;
    }

    public static SC_DialogueManager getInstance()
    {
        return instance;
    }

    private void Start()
    {
        currentDialogue = false;
        dialoguePanel.SetActive(false);
    }

    public void startDialogue(TextAsset inkyText)
    {
        currentStory = new Story(inkyText.text);
        if (sendingData)
        {
            Debug.Log(varNames + " : " + dataVal);
            currentStory.variablesState[varNames] = dataVal;
        }
        currentDialogue = true;
        dialoguePanel.SetActive(true);
        continueDialogue();
    }

    void continueDialogue()
    {
        if (currentStory.canContinue)
        {
            displayText.text = currentStory.Continue();
        }
        else
        {
            dialogueEnded();
        }
    }

    private void Update()
    {
        if (!currentDialogue)
            return;

        if(Input.GetKeyDown(KeyCode.F))
        {
            continueDialogue();
        }
    }

    public void dialogueEnded()
    {
        currentDialogue = false;
        dialoguePanel.SetActive(false);
        displayText.text = "";
        sendingData = false;
        varNames = "";
        dataVal = 0;
    }

    public void sendingVariable(string varName, int data)
    {
        varNames = varName;
        dataVal = data;
        sendingData = true;
    }

}
