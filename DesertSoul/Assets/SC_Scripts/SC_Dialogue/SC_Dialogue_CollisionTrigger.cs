using UnityEngine;
using Yarn.Unity;

public class SC_Dialogue_CollisionTrigger : MonoBehaviour
{

    public string dialogueID;

    public DialogueRunner dialogueRunner;
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (dialogueRunner == null)
        {
            dialogueRunner = FindObjectOfType<DialogueRunner>();
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player entered dialogue trigger");
        if (other.CompareTag("Player"))
        {
            if (!dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.StartDialogue(dialogueID);
                GetComponent<Collider2D>().enabled = false; // Disable the collider to prevent retriggering
                //player.GetComponent<SC_Player_Move>().SetCanMove(false); // Disable player movement during dialogue
            }
        }
    }

    [YarnCommand("enable_movement")]
    public void EndDialogue()
    {
        //player.GetComponent<SC_Player_Move>().SetCanMove(true); // Re-enable player movement after dialogue ends
    }
}
