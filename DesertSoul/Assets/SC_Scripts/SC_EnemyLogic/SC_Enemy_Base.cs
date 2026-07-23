using UnityEngine;
using System.Collections;

public class SC_Enemy_Base : MonoBehaviour
{

    [SerializeField] protected float MAXHealth;
    protected float currentHealth;
    [SerializeField] protected int damage;
    
    [SerializeField] protected SC_RattleSnake snake;
    public AudioClip deathCry;
    //kb vars
    [SerializeField] protected float knockbackDist;
    [SerializeField] protected float totalLerpTime;
    protected float elapsedLerpTime = 0f;

    public bool isSnake = false;

    protected Vector3 kbEndPoint;

    protected bool lerping = false;

    protected Transform lockPoint;

    protected Rigidbody2D rb;

    protected bool locked;

    protected float lockTimer = 0.5f;
    protected float currentLockTimer = 0f;
    protected bool lockCooldown;

    //Launch Vars
    protected bool launchFromRight = true;
    protected float launchTime = 0;
    [SerializeField] protected float totalLaunchTime;
    [SerializeField] protected float launchPower;

    [SerializeField] protected float decelerationMult = 0.95f;
    protected bool decelerationEnabled = false;

    [SerializeField] protected bool bossEnemy = false;

    //REFERENCE GOTTEN AFTER PLAYER HITS ENEMY
    protected GameObject player;

    protected bool canDie = true;

    //spawner stuff, temp, use events later
    SC_WaveRoom roomSpawned;

    [SerializeField] GameObject venture;


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
            if(canDie)
            {
                GameManager.Instance.playSFX(deathCry.name);
                canDie = false;
            }
            StartCoroutine(Die());
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
                if(isSnake)
                {
                    snake.SetCanMove(true);
                }
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
    public virtual bool TakeDamage(SC_Attack_Base attack, Transform carryPoint, GameObject debug, GameObject debug2)
    {
        //debug2.SetActive(true);
        if (!locked)
        {
            Debug.Log("taking damage");

            Debug.Log("hmm");
            
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

    public void Venture()
    {
        venture.SetActive(true);
    }

    public virtual void Knockback(AttackType atkType)
    {
        //TEMPORARY IF STATMENT
        if(isSnake)
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


    protected IEnumerator Die()
    {
        
        if(roomSpawned != null)
        {
            
            roomSpawned.checkWave();
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}

