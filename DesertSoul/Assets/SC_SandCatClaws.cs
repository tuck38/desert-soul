using UnityEngine;

public class SC_SandCatClaws : StateMachineBehaviour
{

    Rigidbody2D rb;
    SC_BossBase bossBase;

    [SerializeField] float attackSpeed;

    [SerializeField] float dashRange;

    private Vector2 target;
    private bool isFacingRight;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(bossBase == null)
        {
            bossBase = animator.GetComponent<SC_BossBase>();
        }
        if(rb == null)
        {
            rb = animator.GetComponent<Rigidbody2D>();
        }
        bossBase.ChangeColor(Color.red, true);

        //the magic numbers are NOT real and they CANNOT hurt me
        //forgive my Johnathan Ferguson

        //bossBase.flipBoss();
        isFacingRight = bossBase.GetDirection();
        if(isFacingRight)
        {
            target = new Vector2(rb.position.x + dashRange, rb.position.y);
        }
        else
        {
            target = new Vector2(rb.position.x - dashRange, rb.position.y);   
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //move

        Vector2 newPos = Vector2.MoveTowards(rb.position, target, attackSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        //raycast to see if hit player or wall

        if(isFacingRight)
        {
            if(Physics2D.Raycast(bossBase.gameObject.transform.position, Vector2.right, bossBase.halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall

                //stun timer, i dont wanna make it rn tho
                animator.SetBool("doAttack1", false);
                animator.SetBool("Moving", true);
            }
            else if(bossBase.transform.position.x <= target.x)
            {
                //range maxxed out

                Debug.Log("huh");
                animator.SetBool("doAttack1", false);
                animator.SetBool("Moving", true);
            }
        }
        else if (!isFacingRight)
        {
            if(Physics2D.Raycast(rb.position, Vector2.left, bossBase.halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall

                //stun timer, i dont wanna make it rn tho

                animator.SetBool("doAttack1", false);
                animator.SetBool("Moving", true);
            }
            else if(rb.position.x >= target.x)
            {
                //range maxxed out

                Debug.Log("huhwuh");
                animator.SetBool("doAttack1", false);
                animator.SetBool("Moving", true);
            }
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
