using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    [SerializeField] private GameObject weapon;
    [SerializeField] private float swingTime = 1f;
    private float swingTimeLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Swing();
    }

    private void Swing()
    {
        if (Input.GetKey(KeyCode.Mouse0) && swingTimeLeft <= 0)
        {
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
            weapon.GetComponent<BoxCollider2D>().enabled = false;
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
