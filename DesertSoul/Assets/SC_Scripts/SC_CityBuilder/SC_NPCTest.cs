using UnityEngine;

public enum NPCTypes
{
    MINER,
    FARMER
}

public class SC_NPCTest : MonoBehaviour
{
    [SerializeField] NPCTypes npcType;
    [SerializeField] string npcName;
    [SerializeField] GameObject dialogue;
    [SerializeField] GameObject winnerDialogue;
    [SerializeField] GameObject loserDialogue;

    private void OnEnable()
    {
        SC_BallCupGame.OnGameOver += MinigameOver;
    }

    private void OnDisable() 
    {
        SC_BallCupGame.OnGameOver -= MinigameOver;
    }

    private void OnMouseDown()
    {
        dialogue.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") dialogue.SetActive(true);
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
        winnerDialogue.SetActive(false);
        loserDialogue.SetActive(false);
    }  

    void MinigameOver(bool isWinner)
    {
        if (isWinner) winnerDialogue.SetActive(true);
        else loserDialogue.SetActive(true);
    }
}
