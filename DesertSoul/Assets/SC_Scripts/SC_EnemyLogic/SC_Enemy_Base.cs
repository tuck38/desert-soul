using UnityEngine;

public class SC_Enemy_Base : MonoBehaviour
{

    [SerializeField] private int MAXHealth;
    private int currentHealth;
    [SerializeField] private int damage;

    private Transform lockPoint;

    private Rigidbody2D rb;

    bool locked;

    private float lockTimer = 0.5f;
    private float currentLockTimer = 0f;
    private bool lockCooldown;

    //Launch Vars
    private bool launchFromRight = true;
    private float launchTime = 0;
    [SerializeField] private float totalLaunchTime;
    [SerializeField] private float launchPower;

    [SerializeField] private float decelerationMult = 0.95f;
    private bool decelerationEnabled = false;

    //REFERENCE GOTTEN AFTER PLAYER HITS ENEMY
    private GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = MAXHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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

        //knockback timer 
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

        if (decelerationEnabled)
        {
            decelerate();
        }
    }

    //takes damage and returns true if the attack killed the enemy
    public bool TakeDamage(SC_Attack_Base attack, Transform carryPoint)
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

    private void Knockback()
    {

        //method 1
        //Vector3 dirVect = gameObject.transform.position - enemy.transform.position;
        //dirVect.Normalize();
        //rb.AddForce(dirVect * launchPower, ForceMode2D.Impulse);
        //rb.linearVelocity = dirVect * launchPower;

        //method 2
        launchTime = totalLaunchTime;

        if (player.transform.position.x >= transform.position.x)
        {
            launchFromRight = true;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            launchFromRight = false;
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
            Knockback();
        }
    }

    private void decelerate()
    {
        Debug.Log(rb.linearVelocity.x);
        rb.linearVelocity = new Vector2 (rb.linearVelocityX * decelerationMult, rb.linearVelocityY);
        Debug.Log(rb.linearVelocity.x);

        if (rb.linearVelocity.x <= 0.2 && rb.linearVelocity.x >= -0.2)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            decelerationEnabled = false;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

