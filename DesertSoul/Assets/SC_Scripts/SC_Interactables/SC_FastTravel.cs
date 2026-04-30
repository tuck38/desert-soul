using UnityEngine;

public class SC_FastTravel : MonoBehaviour
{

    [SerializeField] string goToScene;

    [SerializeField] float buttonHoldTime = 2f;
    private float currentButtonHoldTime = 0f;

    [SerializeField] GameObject interactButton;

    private SC_Player_Move player;

    private bool playerInRange = false;

    private bool holding = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HoldInputs();
        if(holding)
        {
            HoldTimer();
        }
    }

    void HoldInputs()
    {
        if(playerInRange)
        {

            holding = player.IsInteracting();

            if(holding == false)
            {
                currentButtonHoldTime = 0f;
            }
        }
    }

    void HoldTimer()
    {
        if(currentButtonHoldTime < buttonHoldTime)
        {
            currentButtonHoldTime += Time.deltaTime;
        }
        else
        {
            GoToScene();
        }
    }


    void GoToScene()
    {
        GameManager.Instance.LoadNewLevel(goToScene, SC_Areas_Enum.hotdry, SC_Enum_Doors.Bottom, false, true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.GetComponent<SC_Player_Move>();
            playerInRange = true;
            interactButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInRange = false;
            interactButton.SetActive(false);
            holding = false;
            currentButtonHoldTime = 0f;
        }
    }
}
