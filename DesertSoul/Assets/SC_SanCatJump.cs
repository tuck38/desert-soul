using UnityEngine;
using Yarn.Compiler;

public class SC_SanCatJump : StateMachineBehaviour
{

    Rigidbody2D rb;

    [SerializeField] float upForce;
    [SerializeField] float sideForce;

    private SC_BossBase bossBase;

    private bool jumperr;
    private bool isFacingRight;

    private Vector3 center;

    private Vector3 start;

    private Vector3 end;


    [SerializeField] float time = 3f;
    private float currentTime = 0;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;
        jumperr = false;
        if(bossBase == null)
        {
            bossBase = animator.GetComponent<SC_BossBase>();
        }
        if(rb == null)
        {
            rb = animator.GetComponent<Rigidbody2D>();
        }

        bossBase.flipBoss();
        isFacingRight = bossBase.GetDirection();

        center = bossBase.GetPlayerPos().position + bossBase.transform.position * 0.5f;

        //center -= new Vector3(0, 1, 0);

        start = bossBase.transform.position - center;

        end = bossBase.GetPlayerPos().position - center;

        

        /*if(isFacingRight)
        {
            rb.linearVelocity = new Vector2(sideForce, upForce);
        }
        else if (!isFacingRight)
        {
            rb.linearVelocity = new Vector2(-sideForce, upForce);
        }*/
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(jumperr == false && !bossBase.IsGrounded())
        {
            jumperr = true;
        }

        currentTime += Time.deltaTime;

        float fracComplete = currentTime / time;

        bossBase.transform.position = Vector3.Slerp(start, end, fracComplete);
        bossBase.transform.position += center;

        if(jumperr == true && bossBase.IsGrounded())
        {
            animator.SetBool("doAttack2", false);
            rb.linearVelocity = Vector2.zero;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
