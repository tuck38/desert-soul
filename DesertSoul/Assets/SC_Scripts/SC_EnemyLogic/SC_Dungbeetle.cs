using System.Collections;
using UnityEngine;

public class SC_Dungbeetle : SC_Enemy_Attack_Base
{
    [Header("Dungbeetle Values")]
    [SerializeField] GameManager dungBallPrefab;
    [SerializeField] SC_Dungball currentDungball;
    [SerializeField] float attackRange;
    [SerializeField] float dungballInitialForceStrength;

    bool isAttacking = false;
    bool isBallFullyFormed = true;
    Vector3 dungBallDefaultPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.WANDERING;
        nextPoint = point1;
        isGoingOne = true;
        dungBallDefaultPosition = currentDungball.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPos();
        switch (currentState)
        {
            case EnemyState.NONE:
                break;
            case EnemyState.ATTACK:
                if (!isAttacking && isBallFullyFormed) StartCoroutine("AttackCoroutine");
                break;
            case EnemyState.DETECT_PLAYER:
                CheckDistanceFromPlayer();
                break;
            case EnemyState.WANDERING:
                if (!isAttacking) MoveTo();
                break;
        }
    }

    protected override IEnumerator AttackCoroutine()
    {
        currentDungball.transform.parent = null;
        currentDungball.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, dungballInitialForceStrength));
        isBallFullyFormed = false;
        currentState = EnemyState.WANDERING;
        currentDungball = Instantiate(dungBallPrefab, dungBallDefaultPosition, Quaternion.identity, transform).GetComponent<SC_Dungball>();
        currentDungball.transform.localPosition = dungBallDefaultPosition;
        yield return new WaitForSeconds(0f);
    }

    private void CheckDistanceFromPlayer()
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);
        if(dist <= attackRange) currentState = EnemyState.ATTACK;
    }
}
