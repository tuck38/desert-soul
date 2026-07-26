using System.Collections;
using Unity.IO.LowLevel.Unsafe;
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

    //Raycasts
    protected Rigidbody2D rigidbody;

    protected Vector2 currentDirection;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    //Raycast vars
    public float halfWidth;
    public float halfHeight;

    //allows us to have a local, unchaging version of these vars - Ashley
    protected Vector3 realP1;
    protected Vector3 realP2;
    [SerializeField] protected float timeBetweenAttacks = 0.2f;

    protected bool isGoingLeft;
    protected Vector3 nextPoint;
    protected Vector3 preLockOnPoint;

    protected bool isFacingRight = false;

    protected void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentDirection = Vector2.left;
        halfWidth = spriteRenderer.bounds.extents.x;
        halfHeight = spriteRenderer.bounds.extents.y;
    }

    protected virtual void CheckPos()
    {
        float dist = Vector2.Distance(transform.position, nextPoint);
        if (currentState == EnemyState.WANDERING && dist < stopDistancePlatformEdge)
        {
            if (isGoingLeft)
            {
                nextPoint = realP2;
                isGoingLeft = false;
            }
            else
            {
                nextPoint = realP1;
                isGoingLeft = true;
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

    protected virtual void SpriteRotation()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    public virtual void PlayerLost()
    {
        //nextPoint = preLockOnPoint;
        if (isGoingLeft)
        {
            nextPoint = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
        }
        else if(!isGoingLeft)
        {
            nextPoint = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
        }
        currentState = EnemyState.WANDERING;
    }

    protected virtual IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
    }
}
