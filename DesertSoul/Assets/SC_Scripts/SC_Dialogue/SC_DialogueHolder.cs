using UnityEngine;

public class SC_DialogueHolder : MonoBehaviour
{
    [SerializeField] GameObject popUP;
    private bool playerRange;
    public bool playerStartedDialogue;

    [Header("Inky Variable")]
    [SerializeField] bool sendingData;
    [SerializeField] string InkyVariableName;
    [SerializeField] int data;

    [SerializeField] private TextAsset inkyFile;
    private void Awake()
    {
        popUP.SetActive(false);
    }

    private void Update()
    {
        if (playerRange)
        {
            popUP.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F) && !SC_DialogueManager.getInstance().currentDialogue && !playerStartedDialogue)
            {
                playerStartedDialogue = true;
                if (sendingData)
                    SC_DialogueManager.getInstance().sendingVariable(InkyVariableName,SC_DialogueManager.getInstance().testItemCount);
                SC_DialogueManager.getInstance().startDialogue(inkyFile);
            }

            if (!SC_DialogueManager.getInstance().currentDialogue)
                playerStartedDialogue = false;

        }
        else
            popUP.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRange = false;
            playerStartedDialogue = false;
            SC_DialogueManager.getInstance().dialogueEnded();
        }

    }

}
