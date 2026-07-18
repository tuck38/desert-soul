using Unity.VisualScripting;
using UnityEngine;
using Yarn.Compiler;

public class SC_SanCatJump : StateMachineBehaviour
{

    Rigidbody2D rb;

    private SC_BossBase bossBase;

    private bool jumperr;
    private bool isFacingRight;

    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float maxHeight;

    [SerializeField] AnimationCurve curve;

    Vector3 trajectoryStartPoint;
    Vector3 trajectoryEndPoint;

    Vector2 gravityVec = new Vector2(0, -9.8f);

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

        trajectoryStartPoint = bossBase.getBossTransform().position;
        trajectoryEndPoint = bossBase.GetPlayerPos().transform.position;

       /* currentTime = 0f;
        rb.gravityScale = 9.8f;

        //finding initial velocity of boss attack
        UnityEngine.Vector2 displacment = bossBase.GetPlayerPos().position - bossBase.transform.position;

        Vector2 horizontalDisplacement = new Vector2(displacment.x, 0f);

        float verticalDisplacment = displacment.y;

        Vector2 horizontalVelocity = horizontalDisplacement / attackTime;

        float verticalVelocity = (displacment.y - 0.5f * gravityVec.y * attackTime * attackTime)/ attackTime;*/


        //initialVelocity = horizontalVelocity + Vector2.up * verticalVelocity;
        //rb.linearVelocity = horizontalVelocity + Vector2.up * verticalVelocity;



        //bossBase.flipBoss();
        isFacingRight = bossBase.GetDirection();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(jumperr == false && !bossBase.IsGrounded())
        {
            Debug.Log("no, that aint right");
            jumperr = true;
        }

        Vector3 trajectoryRange;

        float nextPositionX = bossBase.gameObject.transform.position.x + attackSpeed * Time.deltaTime;

        float posXNorm = 0;

        float posYNorm = curve.Evaluate(posXNorm);

        float nextPositionY = trajectoryStartPoint.y + posYNorm * maxHeight;

        Vector3 nextPosition = Vector3.zero;

        if(jumperr == true && bossBase.IsGrounded())
        {
            Debug.Log("gwahgwah");
            animator.SetBool("doAttack2", false);
            rb.linearVelocity = Vector2.zero;
        }

        if(isFacingRight)
        {
            if(Physics2D.Raycast(bossBase.gameObject.transform.position, Vector2.right, bossBase.halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall

                //stun timer, i dont wanna make it rn tho

                animator.SetBool("doAttack2", false);
                animator.SetBool("Moving", true);
            }
            trajectoryRange = trajectoryEndPoint - trajectoryStartPoint;

            nextPositionX = bossBase.gameObject.transform.position.x + attackSpeed * Time.deltaTime;

            posXNorm = (nextPositionX - trajectoryStartPoint.x) / trajectoryRange.x;

            posYNorm = curve.Evaluate(posXNorm);

            nextPositionY = trajectoryStartPoint.y + posYNorm * maxHeight;

            nextPosition = new Vector3(nextPositionX, nextPositionY, 0);
        }
        else if(!isFacingRight)
        {
            
            if(Physics2D.Raycast(rb.position, Vector2.left, bossBase.halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall

                //stun timer, i dont wanna make it rn tho

                animator.SetBool("doAttack2", false);
                animator.SetBool("Moving", true);
            }

            trajectoryRange = trajectoryStartPoint - trajectoryEndPoint;

            nextPositionX = bossBase.gameObject.transform.position.x - attackSpeed * Time.deltaTime;

            posXNorm = (trajectoryStartPoint.x - nextPositionX) / trajectoryRange.x;

            posYNorm = curve.Evaluate(posXNorm);

            nextPositionY = trajectoryStartPoint.y + posYNorm * maxHeight;

            nextPosition = new Vector3(nextPositionX, nextPositionY, 0);
        }

        bossBase.transform.position = nextPosition;

        //rb.MovePosition(nextPosition);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossBase.flipBoss();
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
