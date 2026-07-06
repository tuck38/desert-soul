using Unity.VisualScripting;
using UnityEngine;

public class SC_SandCatRun : StateMachineBehaviour
{

    Transform player;

    SC_BossBase bossBase;
    Rigidbody2D rb;

    [SerializeField] float distToAttackClaws = 20;
    [SerializeField] float distToAttackPounce = 30;

    [SerializeField] private float speed = 2.5f; 


    public void Start()
    {
        //this does not run
        Debug.Log("this actually runs");
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if(rb == null)
        {
            rb = animator.GetComponent<Rigidbody2D>();
        }
        if(bossBase == null)
        {
            bossBase = animator.GetComponent<SC_BossBase>();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        Debug.Log(Mathf.Abs(rb.position.x - player.position.x) );
        if(Mathf.Abs(rb.position.x - player.position.x) <= distToAttackClaws)
        {
            animator.SetBool("doAttack1Windup", true);
        }
        else if(Mathf.Abs(rb.position.x - player.position.x) >= distToAttackPounce)
        {
            animator.SetBool("doAttack2Windup", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}
