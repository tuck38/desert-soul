using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    [SerializeField] private int MAXHealth;
    private int currentHealth;

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

    private void Die()
    {
        Destroy(gameObject);
    }
}

