using UnityEngine;

public class SC_SandCatRoar : StateMachineBehaviour
{

    SC_BossBase bossBase;
    Rigidbody2D rb;

    [SerializeField] AudioClip bossRoar;

    [SerializeField] float roarTime;

    float currentTime = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(rb == null)
        {
            rb = animator.GetComponent<Rigidbody2D>();
        }
        if(bossBase == null)
        {
            bossBase = animator.GetComponent<SC_BossBase>();
        }

        //play the roar by calling bassBase, which will call the audio manager

        bossBase.PlaySFX(bossRoar);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(roarTime >= currentTime)
        {
            //could add screen shake here in the future
            currentTime += Time.deltaTime;
        }
        else
        {
            animator.SetBool("Moving", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossBase.BossMusic();
        bossBase.enableHPBar(true);
        bossBase.GetAnimator().SetBool("FightStart", false);
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
