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



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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

        if(isFacingRight)
        {
            rb.linearVelocity = new Vector2(sideForce, upForce);
        }
        else if (!isFacingRight)
        {
            rb.linearVelocity = new Vector2(-sideForce, upForce);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(jumperr == false && !bossBase.IsGrounded())
        {
            jumperr = true;
        }


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
