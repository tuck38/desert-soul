using System.Collections;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;


//THERE ARE 3 DIFFERENT ENEMY SCRIPTS, THEY NEED TO ALL BE CONSOLIDATED BUT I DONT HAVE TIME RN
public class SC_RattleSnake : SC_Enemy_Attack_Base
{
    [Header("Rattlesnake Values")]
    [SerializeField] float chasePlayerSpeed;
    [SerializeField] float stopDistancePlayer = 1f;
    [SerializeField] float timeBetweenApproachingPlayerAndAttacking = 1f;
  
    [SerializeField] Color defaultColor;
    [SerializeField] Color approachColor;
    [SerializeField] Color windUpColor;
    [SerializeField] Color attackColor;

    [SerializeField] float detectionTime;

    private bool canMove = true;

    float currentDetectionTime = 0f;

    [SerializeField] private Animator animator;

    bool isAttacking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
        currentState = EnemyState.WANDERING;
        nextPoint = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
        isGoingLeft = true;
        defaultColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPos();

        switch (currentState)
        {
            case EnemyState.NONE:
                break;
            case EnemyState.APPROACH:
                if (!isAttacking) MoveTo();
                break;
            case EnemyState.ATTACK:
                if(!isAttacking) StartCoroutine("AttackCoroutine");
                break;
            case EnemyState.DETECT_PLAYER:
                detectionTimer();
                break;
            case EnemyState.WANDERING:
                if(!isAttacking) MoveTo();
                break;
        }
    }

    /*protected override IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        Debug.Log("Windup");
       // animator.SetBool("Attack", true);
        animator.SetBool("Moving", false);
        yield return new WaitForSeconds(timeBetweenApproachingPlayerAndAttacking);
        if(Vector2.Distance(transform.position, nextPoint) < stopDistancePlayer)
        {
            Debug.Log("Attack Hit");
            player.GetComponent<SC_Player_Prop>().TakeDamage(GetComponent<SC_Enemy_Base>().GetDamage());
        }
        else Debug.Log("Attack Missed");
        //yield return new WaitForSeconds(timeBetweenAttacks);
        if(currentState == EnemyState.ATTACK) currentState = EnemyState.APPROACH;
        isAttacking = false;
    }*/

    

    private void detectionTimer()
    {
        if(currentDetectionTime < detectionTime)
        {
            currentDetectionTime += Time.deltaTime;
        }
        else if(currentDetectionTime >= detectionTime)
        {
            animator.SetBool("detected", false);
            currentState = EnemyState.APPROACH;
            animator.SetBool("Attack", true);
            currentDetectionTime = 0;
        }
    }

    public override void PlayerDetected(GameObject playerObj)
    {
        if(currentState == EnemyState.WANDERING)
        {
            animator.SetBool("detected", true);
            player = playerObj;
            preLockOnPoint = nextPoint;
            nextPoint = player.transform.position;
            currentState = EnemyState.DETECT_PLAYER;
        }
    }

    //TODO:
    //Set up racast
    //change Moveto functions to always move enemy 5f ahead in its direction, stopped by the raycast
    protected override void CheckPos()
    {
        Vector2 rightPos = transform.position;
        Vector2 leftPos = transform.position;
        rightPos.x += halfWidth;
        leftPos.x -= halfWidth;

        float dist = Vector2.Distance(transform.position, nextPoint);


        if((currentState == EnemyState.WANDERING || currentState == EnemyState.APPROACH) && nextPoint.x > transform.position.x)
        {
            //Going Right
            isGoingLeft = false;
            if(Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall
                isGoingLeft = !isGoingLeft;
                SpriteRotation();
            }
            else if(!Physics2D.Raycast(rightPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                //No ledge below
                isGoingLeft = !isGoingLeft;
                SpriteRotation();
            }
        }
        else if((currentState == EnemyState.WANDERING || currentState == EnemyState.APPROACH) && nextPoint.x < transform.position.x)
        {
            //Going Left
            isGoingLeft = true;
            if(Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall
                isGoingLeft = !isGoingLeft;
                SpriteRotation();
            }
            else if(!Physics2D.Raycast(leftPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                //No ledge below
                isGoingLeft = !isGoingLeft;
                SpriteRotation();
            }

        }
        /*else if (currentState == EnemyState.APPROACH && dist < stopDistancePlayer)
        {
            Debug.Log("Close Enough To Attack");
            currentState = EnemyState.ATTACK;
        }*/
    }

    //I loooove tech debt
    public void SetCanMove(bool move)
    {
        canMove = move;
    }

    protected override void MoveTo()
    {
        if (canMove == true)
        {
            animator.SetBool("Moving", true);
            float initialY = transform.position.y;
            if(currentState == EnemyState.WANDERING)
            {
                if (isGoingLeft)
                {
                    nextPoint = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                }
                else if(!isGoingLeft)
                {
                    nextPoint = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
                }
            }
            Vector2 newXPosition = Vector2.MoveTowards(transform.position, nextPoint, currentState == EnemyState.WANDERING ? defaultSpeed * Time.deltaTime : chasePlayerSpeed * Time.deltaTime);
            //SpriteRotation(newXPosition.x);
            //Debug.Log(newXPosition);
            transform.position = new Vector2(newXPosition.x, initialY);
        }
    }
}
