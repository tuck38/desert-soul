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


    [SerializeField] float attackTime = 3f;

    //math (ew)

    float accelerationX = 0;

    float accelerationY = -9.8f;

    [SerializeField] float angle = 30f;

    Vector2 launchVelocity;
    
    float initialVelocity;

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

        Vector3 gravity = new Vector3(0,-9.8f,0);



        bossBase.flipBoss();
        isFacingRight = bossBase.GetDirection();
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
