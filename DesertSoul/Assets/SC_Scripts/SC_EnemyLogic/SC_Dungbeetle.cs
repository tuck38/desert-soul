using System.Collections;
using UnityEngine;

public class SC_Dungbeetle : SC_Enemy_Attack_Base
{
    [Header("Dungbeetle Values")]
    [SerializeField] GameObject dungBallPrefab;
    [SerializeField] SC_Dungball currentDungball;
    [SerializeField] float attackRange;
    [SerializeField] float dungballInitialForceStrength;

    [SerializeField] Color defaultColor;
    [SerializeField] Color playerDetectedColor;
    [SerializeField] Color attackColor;

    bool isAttacking = false;
    bool isBallFullyFormed = true;
    Vector3 dungBallDefaultPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.WANDERING;
        nextPoint = point1;
        isGoingOne = true;
        dungBallDefaultPosition = currentDungball.transform.parent.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        isBallFullyFormed = currentDungball.IsFullyFormed();
        CheckPos();
        switch (currentState)
        {
            case EnemyState.NONE:
                break;
            case EnemyState.ATTACK:
                GetComponent<SpriteRenderer>().color = attackColor;
                if (!isAttacking && isBallFullyFormed) StartCoroutine("AttackCoroutine");
                break;
            case EnemyState.DETECT_PLAYER:
                GetComponent<SpriteRenderer>().color = playerDetectedColor;
                CheckDistanceFromPlayer();
                break;
            case EnemyState.WANDERING:
                GetComponent<SpriteRenderer>().color = defaultColor;
                if (!isAttacking) MoveTo();
                break;
        }
    }

    protected override IEnumerator AttackCoroutine()
    {
        currentDungball.transform.parent.parent = null;
        currentDungball.GetComponent<Rigidbody2D>().AddForce(transform.right * dungballInitialForceStrength);
        isBallFullyFormed = false;
        currentState = EnemyState.WANDERING;
        currentDungball = Instantiate(dungBallPrefab, dungBallDefaultPosition, Quaternion.identity, transform).transform.GetChild(0).GetComponent<SC_Dungball>();
        currentDungball.transform.parent.localPosition = dungBallDefaultPosition;
        yield return new WaitForSeconds(0f);
    }

    public override void PlayerDetected(GameObject playerObj)
    {
        Debug.Log("Player detected");
        if(isBallFullyFormed)
        {
            player = playerObj;
            preLockOnPoint = nextPoint;
            nextPoint = player.transform;
            currentState = EnemyState.DETECT_PLAYER;
        }
    }

    private void CheckDistanceFromPlayer()
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);
        if(dist <= attackRange) currentState = EnemyState.ATTACK;
    }
}
