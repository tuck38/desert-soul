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

    public int GetDamage()
    { 
        return damage; 
    }
    
    public void setLocked(bool shouldLock)
    {
        locked = shouldLock;
        if (!locked)
        {
            lockCooldown = true;
            currentLockTimer = lockTimer;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

