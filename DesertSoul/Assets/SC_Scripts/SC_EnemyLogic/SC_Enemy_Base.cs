using UnityEngine;

public class SC_Enemy_Base : MonoBehaviour
{

    [SerializeField] protected float MAXHealth;
    protected float currentHealth;
    [SerializeField] private int damage;
    
    [SerializeField] private SC_RattleSnake snake;

    //kb vars
    [SerializeField] private float knockbackDist;
    [SerializeField] private float totalLerpTime;
    private float elapsedLerpTime = 0f;

    Vector3 kbEndPoint;

    bool lerping = false;

    private Transform lockPoint;

    private Rigidbody2D rb;

    protected bool locked;

    private float lockTimer = 0.5f;
    private float currentLockTimer = 0f;
    protected bool lockCooldown;

    //Launch Vars
    private bool launchFromRight = true;
    private float launchTime = 0;
    [SerializeField] private float totalLaunchTime;
    [SerializeField] private float launchPower;

    [SerializeField] private float decelerationMult = 0.95f;
    private bool decelerationEnabled = false;

    [SerializeField] bool bossEnemy = false;

    //REFERENCE GOTTEN AFTER PLAYER HITS ENEMY
    private GameObject player;

    //spawner stuff, temp, use events later
    SC_WaveRoom roomSpawned;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentHealth = MAXHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (currentHealth <= 0)
        {
            Die();
        }

        if(currentLockTimer > 0)
        {
            currentLockTimer -= Time.deltaTime;
            if (currentLockTimer <= 0)
            {
                lockCooldown = false;
            }
        }

        if (locked)
        {
            gameObject.transform.position = new Vector3(lockPoint.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        //drill knockback timer 
        if(launchTime > 0)
        {         
            //This system works for now, but doesent use gravity and feels floaty
            if (launchFromRight)
            {
                rb.linearVelocity = new Vector2(-launchPower, launchPower / 2);
            }
            else
            {
                rb.linearVelocity = new Vector2(launchPower, launchPower / 2);
            }

            launchTime -= Time.deltaTime;
            if(launchTime <= 0)
            {
               decelerationEnabled = true;
            }
        }

        //this is for knockback lerping, just doing a quick dirty implementation
        if(lerping)
        {
            elapsedLerpTime += Time.deltaTime;
            float percentageComplete = elapsedLerpTime / totalLerpTime;

            transform.position = Vector3.Lerp(transform.position, kbEndPoint, percentageComplete);

            //if statment of doom and dispair
            if(transform.position == kbEndPoint)
            {
                lerping = false;
                snake.SetCanMove(true);
            }
        }

        if (decelerationEnabled)
        {
            decelerate();
        }
    }

    public bool IsBoss()
    {
        return bossEnemy;
    }

    //takes damage and returns true if the attack killed the enemy
    public virtual bool TakeDamage(SC_Attack_Base attack, Transform carryPoint)
    {
        if (!locked)
        {
            currentHealth -= attack.getDamage();
            if (currentHealth <= 0)
            {
                return true;
            }

            if (attack.shouldCarry() && !lockCooldown)
            {
                locked = true;
                lockPoint = carryPoint;
            }
        }


        return false;
    }

    public void setMommaSpawner(SC_WaveRoom lockRoom)
    {
        roomSpawned = lockRoom;
    }

    public void Knockback(AttackType atkType)
    {

        //method 1
        //Vector3 dirVect = gameObject.transform.position - enemy.transform.position;
        //dirVect.Normalize();
        //rb.AddForce(dirVect * launchPower, ForceMode2D.Impulse);
        //rb.linearVelocity = dirVect * launchPower;

        //method 2
        if(atkType == AttackType.drillSide)
        {
            launchTime = totalLaunchTime;
        }

        Vector3 target = transform.position;

        if (player.transform.position.x >= transform.position.x)
        {
            launchFromRight = true;
            target = new Vector3(transform.position.x - knockbackDist, transform.position.y, transform.position.z);
        }
        else if (player.transform.position.x < transform.position.x)
        {
            launchFromRight = false;
            target = new Vector3(transform.position.x + knockbackDist, transform.position.y, transform.position.z);
        }

        if(atkType == AttackType.primary)
        {
            if(bossEnemy != true)
            {
                //ima make this a lerp
                lerping = true;
                kbEndPoint = target;
                snake.SetCanMove(false);
                //transform.position = target;
            }
        }
    }

    public int GetDamage()
    { 
        return damage; 
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
    
    public void setLocked(bool shouldLock)
    {
        locked = shouldLock;
        if (!locked)
        {
            lockCooldown = true;
            currentLockTimer = lockTimer;
            Knockback(AttackType.drillSide);
        }
    }

    private void decelerate()
    {
        rb.linearVelocity = new Vector2 (rb.linearVelocityX * decelerationMult, rb.linearVelocityY);

        if (rb.linearVelocity.x <= 0.2 && rb.linearVelocity.x >= -0.2)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            decelerationEnabled = false;
        }
    }


    private void Die()
    {
        if(roomSpawned != null)
        {
            roomSpawned.checkWave();
        }
        Destroy(gameObject);
    }
}

