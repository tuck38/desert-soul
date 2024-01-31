using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    [SerializeField] private GameObject weapon;
    [SerializeField] private float swingTime = 1f;
    [SerializeField] private int damage = 1;
    private float swingTimeLeft;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Swing();
    }

    public bool getAttacking()
    {
        return isAttacking;
    }

    public int getDamage()
    {
        return damage;
    }

    private void Swing()
    {
        if (Input.GetKey(KeyCode.Mouse0) && swingTimeLeft <= 0 && Input.GetKey(KeyCode.UpArrow))
        {
            isAttacking = true;
            weapon.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            weapon.transform.localPosition = new Vector3(0f, 1f, 0f);
            weapon.GetComponent<BoxCollider2D>().enabled = true;
            weapon.GetComponent<SpriteRenderer>().enabled = true;
            swingTimeLeft = swingTime;
        }

        if (Input.GetKey(KeyCode.Mouse0) && swingTimeLeft <= 0)
        {
            isAttacking = true;
            weapon.GetComponent<BoxCollider2D>().enabled = true;
            weapon.GetComponent<SpriteRenderer>().enabled = true;
            swingTimeLeft = swingTime;
        }

        if(swingTimeLeft > 0)
        {
            swingTimeLeft -= Time.deltaTime;
        }
        else
        {
            isAttacking = false;
            weapon.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            weapon.transform.localPosition = new Vector3(1f, 0f, 0f);
            weapon.GetComponent<BoxCollider2D>().enabled = false;
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Hitbox")
            {
                collision.gameObject.GetComponent<EnemyProperties>().takeDamage(damage);
            }
        }
    }
}
