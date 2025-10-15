using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    NONE,
    WANDERING,
    APPROACH,
    ATTACK
}

public class SC_RattleSnake : MonoBehaviour
{
    EnemyState currentState = EnemyState.NONE;

    [SerializeField] GameObject player;
    [SerializeField] float defaultSpeed;
    [SerializeField] float chasePlayerSpeed;
    [SerializeField] float stopDistancePlatformEdge = 0.5f;
    [SerializeField] float stopDistancePlayer = 1f;
    [SerializeField] float timeBetweenApproachingPlayerAndAttacking = 1f;
    [SerializeField] float timeBetweenAttacks = 0.2f;
    [SerializeField] Transform point1;
    [SerializeField] Transform point2;
  
    [SerializeField] Color defaultColor;
    [SerializeField] Color approachColor;
    [SerializeField] Color windUpColor;
    [SerializeField] Color attackColor;

    bool isGoingOne, isAttacking = false;
    private Transform nextPoint;
    private Transform preLockOnPoint;

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
            case EnemyState.WANDERING:
                GetComponent<SpriteRenderer>().color = defaultColor;
                if(!isAttacking) MoveTo();
                break;
        }
        //currentState = CheckState();
    }

    EnemyState CheckState()
    {
        if (player) return EnemyState.APPROACH;
        if (currentState == EnemyState.APPROACH && Vector2.Distance(transform.position, player.transform.position) < stopDistancePlayer) return EnemyState.ATTACK;
        return EnemyState.WANDERING;
    }

    IEnumerator AttackCoroutine()
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

    private void CheckPos()
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

    private void MoveTo()
    {
        float initialY = transform.position.y;
        Vector2 newXPosition = Vector2.MoveTowards(transform.position, nextPoint.position, currentState == EnemyState.WANDERING ? defaultSpeed * Time.deltaTime : chasePlayerSpeed * Time.deltaTime);
        //Debug.Log(newXPosition);
        transform.position = new Vector2(newXPosition.x, initialY);
    }

    public void PlayerDetected(GameObject playerObj)
    {
        Debug.Log("Player detected");
        player = playerObj;
        preLockOnPoint = nextPoint;
        nextPoint = player.transform;
        currentState = EnemyState.APPROACH;
    }

    public void PlayerLost()
    {
        Debug.Log("Player lost");
        player = null;
        nextPoint = preLockOnPoint;
        currentState = EnemyState.WANDERING;
    }

}
