using UnityEngine;

public class SC_Enemy_Base : MonoBehaviour
{

    [SerializeField] private int MAXHealth;
    private int currentHealth;
    [SerializeField] private int damage;

    private Transform lockPoint;

    private Rigidbody2D rb;

    bool locked;
    float lockTime;


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
        if (locked)
        {
            gameObject.transform.position = lockPoint.position;
        }

        if(lockTime > 0)
        {
            lockTime -= Time.deltaTime;
        }
        else
        {
            locked = false;
        }
    }

    //takes damage and returns true if the attack killed the enemy
    public bool TakeDamage(SC_Attack_Base attack, Transform carryPoint)
    {
        currentHealth -= attack.getDamage();
        if (currentHealth <= 0)
        {
            return true;
        }
        if (attack.shouldCarry())
        {
            locked = true;
            lockPoint = carryPoint;
            lockTime = attack.getTime();
            Debug.Log("go away spongebob");

        }
        return false;
    }

    public int GetDamage()
    { 
        return damage; 
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

