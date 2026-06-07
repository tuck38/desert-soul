using System.Collections;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private Animator animator;

    bool isAttacking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
        currentState = EnemyState.WANDERING;
        realP1 = new Vector3(point1.position.x, point1.position.y, point1.position.z);
        realP2 = new Vector3(point2.position.x, point2.position.y, point2.position.z);
        nextPoint = realP1;
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
                currentState = EnemyState.APPROACH;
                break;
            case EnemyState.WANDERING:
                if(!isAttacking) MoveTo();
                break;
        }
    }

    protected override IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        Debug.Log("Windup");
        animator.SetBool("Attack", true);
        animator.SetBool("Moving", false);
        yield return new WaitForSeconds(timeBetweenApproachingPlayerAndAttacking);
        if(Vector2.Distance(transform.position, nextPoint) < stopDistancePlayer)
        {
            Debug.Log("Attack Hit");
            player.GetComponent<SC_Player_Prop>().TakeDamage(GetComponent<SC_Enemy_Base>().GetDamage());
        }
        else Debug.Log("Attack Missed");
        yield return new WaitForSeconds(timeBetweenAttacks);
        if(currentState == EnemyState.ATTACK) currentState = EnemyState.APPROACH;
        animator.SetBool("Attack", false);
        animator.SetBool("Moving", true);
        isAttacking = false;
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


        if(nextPoint.x > transform.position.x)
        {
            //Going Right
            isGoingLeft = false;
            if(Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall
            }
            else if(!Physics2D.Raycast(rightPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                //No ledge below
            }
        }
        else if(nextPoint.x < transform.position.x)
        {
            //Going Left
            isGoingLeft = true;
            if(Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall
            }
            else if(!Physics2D.Raycast(leftPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                //No ledge below
            }

        }


        float dist = Vector2.Distance(transform.position, nextPoint);
        if (currentState == EnemyState.WANDERING && dist < stopDistancePlatformEdge)
        {
            if (isGoingLeft)
            {
                nextPoint = realP2;
                isGoingLeft = false;
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else
            {
                nextPoint = realP1;
                isGoingLeft = true;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
        else if (currentState == EnemyState.APPROACH && dist < stopDistancePlayer)
        {
            Debug.Log("Close Enough To Attack");
            currentState = EnemyState.ATTACK;
        }
    }

    protected override void MoveTo()
    {
        animator.SetBool("Moving", true);
        float initialY = transform.position.y;
        Vector2 newXPosition = Vector2.MoveTowards(transform.position, nextPoint, currentState == EnemyState.WANDERING ? defaultSpeed * Time.deltaTime : chasePlayerSpeed * Time.deltaTime);
        //SpriteRotation(newXPosition.x);
        //Debug.Log(newXPosition);
        transform.position = new Vector2(newXPosition.x, initialY);
    }
}
