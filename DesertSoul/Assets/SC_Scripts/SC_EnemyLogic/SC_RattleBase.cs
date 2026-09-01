using UnityEngine;
using System.Collections;

public class SC_RattleBase : MonoBehaviour
{

    [SerializeField] protected float MAXHealth;
    protected float currentHealth;
    [SerializeField] protected int damage;
    
    [SerializeField] protected SC_RattleSnake snake;
    public AudioClip deathCry;
    
    [SerializeField] AudioClip hurtSound;
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

    Color defaultColor;
    [SerializeField] Color damagedColor;

    private SpriteRenderer sprite;

    [SerializeField] GameObject snakeDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = MAXHealth;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        defaultColor = sprite.color;
    }

    // Update is called once per frame
    void Update()
    {

        if (currentHealth <= 0)
        {
            if(canDie)
            {
                GameManager.Instance.playSFX(deathCry.name, true);
                canDie = false;
                Die();
            }
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

            if(Physics2D.Raycast(transform.position, Vector2.right, snake.halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall
                lerping = false;
                snake.SetCanMove(true);
            }

            if(Physics2D.Raycast(transform.position, Vector2.left, snake.halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall
                lerping = false;
                snake.SetCanMove(true);
            }


            elapsedLerpTime += Time.deltaTime;
            float percentageComplete = elapsedLerpTime / totalLerpTime;

            transform.position = Vector3.Lerp(transform.position, kbEndPoint, percentageComplete);

            //if statment of doom and dispair
            if(transform.position == kbEndPoint)
            {
                lerping = false;
                sprite.color = defaultColor;
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

    //takes damage and returns true if the attack killed the enemy
    public bool TakeDamage(SC_Attack_Base attack, Transform carryPoint, GameObject debug, GameObject debug2)
    {
        if (!locked)
        {
            
            currentHealth -= attack.getDamage();

            GameManager.Instance.playSFX(hurtSound.name, true);
            snake.ApproachPlayer();
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

    public bool IsBoss()
    {
        return bossEnemy;
    }


    public void Knockback(AttackType atkType, float kbDist)
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

        knockbackDist = kbDist;

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
                sprite.color = damagedColor;
                kbEndPoint = target;
                snake.SetCanMove(false);
                //transform.position = target;
            }
        }
        }
    }

    public void setMommaSpawner(SC_WaveRoom lockRoom)
    {
        roomSpawned = lockRoom;
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
            Knockback(AttackType.drillSide, knockbackDist);
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

    void Die()
    {
        Debug.Log("???");
        if(roomSpawned != null)
        {
            
            roomSpawned.checkWave();
        }

        //yield return new WaitForSeconds(0.2f);
        SC_SnakeDead dead = Instantiate(snakeDead, gameObject.transform.position, gameObject.transform.rotation).GetComponent<SC_SnakeDead>();
        dead.knockBack(launchFromRight);
        Destroy(gameObject);
        
    }
}
