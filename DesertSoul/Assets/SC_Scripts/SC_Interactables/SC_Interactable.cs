using UnityEngine;
using UnityEngine.UI;

public class SC_Interactable : MonoBehaviour
{
    //is good, just needs smthn for the graphics + text changing based on set inputs
    [SerializeField] protected string goToScene;
    protected SC_Player_Prop playerProp;
    [SerializeField] float buttonHoldTime = 2f;
    private float currentButtonHoldTime = 0f;

    [SerializeField] bool needsHold = false;

    [SerializeField] protected GameObject interactButton;

    [SerializeField] string buttonText;

    protected SC_Player_Move player;

    private bool playerInRange = false;

    private bool pressing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Inputs();
        if(pressing)
        {
            if(needsHold)
            {
                HoldTimer();
            }
            else
            {
                DoAction();
            }
        }
    }

    void Inputs()
    {
        if(playerInRange)
        {
            pressing = player.IsInteracting();

            if(pressing == false)
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
            DoAction();
        }
    }


    protected virtual void DoAction()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.GetComponent<SC_Player_Move>();
            playerProp = collision.GetComponent<SC_Player_Prop>();
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
            pressing = false;
            currentButtonHoldTime = 0f;
        }
    }
}
