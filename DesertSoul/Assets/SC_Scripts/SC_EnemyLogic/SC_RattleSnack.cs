using System.Collections;
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

    bool isAttacking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.WANDERING;
        nextPoint = point1;
        isGoingOne = true;
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
                GetComponent<SpriteRenderer>().color = approachColor;
                if (!isAttacking) MoveTo();
                break;
            case EnemyState.ATTACK:
                if(!isAttacking) StartCoroutine("AttackCoroutine");
                break;
            case EnemyState.DETECT_PLAYER:
                currentState = EnemyState.APPROACH;
                break;
            case EnemyState.WANDERING:
                GetComponent<SpriteRenderer>().color = defaultColor;
                if(!isAttacking) MoveTo();
                break;
        }
    }

    protected override IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        Debug.Log("Windup");
        GetComponent<SpriteRenderer>().color = windUpColor;
        yield return new WaitForSeconds(timeBetweenApproachingPlayerAndAttacking);
        GetComponent<SpriteRenderer>().color = attackColor;
        if(Vector2.Distance(transform.position, nextPoint.position) < stopDistancePlayer)
        {
            Debug.Log("Attack Hit");
            player.GetComponent<SC_Player_Prop>().TakeDamage(GetComponent<SC_Enemy_Base>().GetDamage());
        }
        else Debug.Log("Attack Missed");
        yield return new WaitForSeconds(timeBetweenAttacks);
        if(currentState == EnemyState.ATTACK) currentState = EnemyState.APPROACH;
        isAttacking = false;
    }

    protected override void CheckPos()
    {
        float dist = Vector2.Distance(transform.position, nextPoint.position);
        if (currentState == EnemyState.WANDERING && dist < stopDistancePlatformEdge)
        {
            if (isGoingOne)
            {
                nextPoint = point2;
                isGoingOne = false;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180)); 
            }
            else
            {
                nextPoint = point1;
                isGoingOne = true;
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
        float initialY = transform.position.y;
        Vector2 newXPosition = Vector2.MoveTowards(transform.position, nextPoint.position, currentState == EnemyState.WANDERING ? defaultSpeed * Time.deltaTime : chasePlayerSpeed * Time.deltaTime);
        //Debug.Log(newXPosition);
        transform.position = new Vector2(newXPosition.x, initialY);
    }
}
