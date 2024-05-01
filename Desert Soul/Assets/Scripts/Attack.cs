using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    [SerializeField] private GameObject weapon;
    [SerializeField] private float swingTime = 1f;
    [SerializeField] private int damage = 1;
    private bool swingDown = false;
    private float swingVert = 0;
    private PlayerMovement movement;
    private float swingTimeLeft;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        movement = gameObject.GetComponentInParent<PlayerMovement>();
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

    public bool getSwingDown()
    {
        return swingDown;
    }

    public void setSwingVert(float swing)
    {
        
        this.swingVert = swing;
    }

    public int getDamage()
    {
        return damage;
    }

    private void Swing()
    {

            if (Input.GetKey(KeyCode.Mouse0) && swingTimeLeft <= 0 && swingVert > 0f)
            {
                isAttacking = true;
                weapon.transform.eulerAngles = new Vector3(0f, 0f, 90f);
                weapon.transform.localPosition = new Vector3(0f, 1f, 0f);
                weapon.GetComponent<BoxCollider2D>().enabled = true;
                weapon.GetComponent<SpriteRenderer>().enabled = true;
                swingTimeLeft = swingTime;
            }

            if (Input.GetKey(KeyCode.Mouse0) && swingTimeLeft <= 0 && swingVert < 0f && !movement.IsGrounded())
            {
                isAttacking = true;
                swingDown = true;
                weapon.transform.eulerAngles = new Vector3(0f, 0f, -90f);
                weapon.transform.localPosition = new Vector3(0f, -1f, 0f);
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

        if (swingTimeLeft > 0)
        {
            swingTimeLeft -= Time.deltaTime;
        }
        else
        {
            isAttacking = false;
            swingDown = false;
            weapon.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            weapon.transform.localPosition = new Vector3(1f, 0f, 0f);
            weapon.GetComponent<BoxCollider2D>().enabled = false;
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }


    //Useless, look in EnemyProperties script!!!
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
