using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{

    [SerializeField] private int maxHealth = 10;
    private int health;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getDamage()
    {
        return damage;
    }

    public int getHealth()
    {
        return health;
    }

    private void checkHealth()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
        checkHealth();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Hurtbox")
            {
                takeDamage(collision.gameObject.GetComponent<Attack>().getDamage());
                Knockback(collision.gameObject);
            }
        }
    }

    private void Knockback(GameObject source)
    {
        bool left;
        //figuring out how to do this
    }
}
