using System.Collections;
using UnityEngine;

public enum EnemyState
{
    NONE,
    WANDERING,
    DETECT_PLAYER,
    APPROACH,
    ATTACK
}

public class SC_Enemy_Attack_Base : MonoBehaviour
{
    protected EnemyState currentState = EnemyState.NONE;

    [Header("Default Values")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected float defaultSpeed;
    [SerializeField] protected float stopDistancePlatformEdge = 0.5f;
    [SerializeField] protected Transform point1;
    [SerializeField] protected Transform point2;

    [SerializeField] protected float timeBetweenAttacks = 0.2f;

    protected bool isGoingOne;
    protected Transform nextPoint;
    protected Transform preLockOnPoint;

    protected virtual void CheckPos()
    {
        float dist = Vector2.Distance(transform.position, nextPoint.position);
        if (currentState == EnemyState.WANDERING && dist < stopDistancePlatformEdge)
        {
            if (isGoingOne)
            {
                nextPoint = point2;
                isGoingOne = false;
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else
            {
                nextPoint = point1;
                isGoingOne = true;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }
    }

    protected virtual void MoveTo()
    {
        float initialY = transform.position.y;
        Vector2 newXPosition = Vector2.MoveTowards(transform.position, nextPoint.position, defaultSpeed * Time.deltaTime);
        //Debug.Log(newXPosition);
        transform.position = new Vector2(newXPosition.x, initialY);
    }

    public virtual void PlayerDetected(GameObject playerObj)
    {
        Debug.Log("Player detected");
        player = playerObj;
        preLockOnPoint = nextPoint;
        nextPoint = player.transform;
        currentState = EnemyState.DETECT_PLAYER;
    }

    public virtual void PlayerLost()
    {
        Debug.Log("Player lost");
        player = null;
        nextPoint = preLockOnPoint;
        currentState = EnemyState.WANDERING;
    }

    protected virtual IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
    }
}
