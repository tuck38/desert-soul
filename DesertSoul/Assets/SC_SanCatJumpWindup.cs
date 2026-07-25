using UnityEngine;

public class SC_SanCatJumpWindup : StateMachineBehaviour
{

    SC_BossBase bossBase;

    [SerializeField] float windUpTime;
    private float currentWindUpTime = 0;

    [SerializeField] AudioClip windup;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animation/sprite change
    
        if(bossBase == null)
        {
            bossBase = animator.GetComponent<SC_BossBase>();
        }

        bossBase.SetLastPlayerPos();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(currentWindUpTime < windUpTime)
        {
            currentWindUpTime += Time.deltaTime;
        }
        else
        {
            currentWindUpTime = 0;
            bossBase.ChangeColor(Color.orange, true);
            animator.SetBool("doAttack2Windup", false);
            animator.SetBool("doAttack2", true);
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
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
