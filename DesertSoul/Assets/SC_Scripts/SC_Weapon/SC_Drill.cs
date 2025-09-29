using UnityEngine;

public class SC_Drill : MonoBehaviour
{
    //Venture overwatch
    private Rigidbody2D rb;
    private SC_Player_Move playerMove;
    private Animator animator;


    [SerializeField] private int damage;
    [SerializeField] private float timeDrillin = .2f;
    [SerializeField] private float drillSpeed = 5f;
    [SerializeField] private float drillCooldown = 1f;
    [SerializeField] private SC_HurtBox hurtbox;
    [SerializeField] private SC_Attack_Base attack;



    private float currentDrillCooldown = 0f; 
    private float currentTimeDrillin = 0f;

    private bool drillin = false;

    private bool playerGrounded;



    //:3
    private Vector2 shmovement;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerMove = gameObject.GetComponent<SC_Player_Move>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (drillin)
        {
            if (currentTimeDrillin <= timeDrillin)
            {
                currentTimeDrillin += Time.deltaTime;
                playerMove.Move(shmovement, drillSpeed, false);
            }
            else
            {
                //done drillin
                drillin = false;
                animator.SetBool("Drill", false);
                currentDrillCooldown = drillCooldown;
                playerMove.SetCanMove(true);
            }
        }
        else
        { 
            if (currentDrillCooldown > 0)
            {
                currentDrillCooldown -= Time.deltaTime;
            }
        }
    }

    public void Drill(bool isGrounded, bool isFacingRight)
    {
        //Drill Animation switch here 

        playerGrounded = isGrounded;

        if(isGrounded & currentDrillCooldown <= 0)
        {
            animator.SetBool("Drill", true);
            drillin = true;
            hurtbox.currentAttack = attack;
            currentTimeDrillin = 0f;
            playerMove.SetCanMove(false);
            if (isFacingRight)
            {
                shmovement = new Vector2(1, 0);
            }
            else
            {
                shmovement = new Vector2(-1, 0);
            }
        }
    }
}
