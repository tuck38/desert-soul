using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private int maxHealth = 10;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
        if (health < 0)
        {
            die();
        }
    }

    private void die()
    {
        Debug.Log("L RIP bozo");
        //kills you
        //havent done this yet
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Hitbox")
        {
            Debug.Log("oh no!!!");
            EnemyProperties enemyScript = GetComponent<EnemyProperties>();
            takeDamage(enemyScript.getDamage());
        }
    }
}
