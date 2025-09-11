using UnityEngine;

public class SC_Enemy_Base : MonoBehaviour
{

    [SerializeField] private int MAXHealth;
    private int currentHealth;
    [SerializeField] private int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = MAXHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //takes damage and returns true if the attack killed the enemy
    public bool TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            return true;
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

