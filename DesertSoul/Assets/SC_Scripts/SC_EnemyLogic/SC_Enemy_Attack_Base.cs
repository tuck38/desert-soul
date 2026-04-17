using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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

    //allows us to have a local, unchaging version of these vars - Ashley
    protected Vector3 realP1;
    protected Vector3 realP2;
    [SerializeField] protected float timeBetweenAttacks = 0.2f;

    protected bool isGoingOne;
    protected Vector3 nextPoint;
    protected Vector3 preLockOnPoint;

    protected bool isFacingRight = false;

    protected virtual void CheckPos()
    {
        float dist = Vector2.Distance(transform.position, nextPoint);
        if (currentState == EnemyState.WANDERING && dist < stopDistancePlatformEdge)
        {
            if (isGoingOne)
            {
                nextPoint = realP2;
                isGoingOne = false;
            }
            else
            {
                nextPoint = realP1;
                isGoingOne = true;
            }
        }
    }

    protected virtual void MoveTo()
    {
        float initialY = transform.position.y;
        Vector2 newXPosition = Vector2.MoveTowards(transform.position, nextPoint, defaultSpeed * Time.deltaTime);
        //Debug.Log(newXPosition);
        transform.position = new Vector2(newXPosition.x, initialY);
    }

    public virtual void PlayerDetected(GameObject playerObj)
    {
        if(currentState == EnemyState.WANDERING)
        {
            Debug.Log("Player detected");
            player = playerObj;
            preLockOnPoint = nextPoint;
            nextPoint = player.transform.position;
            currentState = EnemyState.DETECT_PLAYER;
        }
    }

    protected virtual void SpriteRotation(float GoToX)
    {
        if(transform.position.x > GoToX && isFacingRight || transform.position.x < GoToX && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
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
