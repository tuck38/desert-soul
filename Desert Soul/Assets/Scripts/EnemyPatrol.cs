using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint.transform == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.3f)
        {
            Debug.Log("hai!!!!");
            if(Vector2.Distance(transform.position, pointA.transform.position) < 0.3f)
            {
                currentPoint = pointB.transform;
            }
            else if(Vector2.Distance(transform.position, pointB.transform.position) < 0.3f)
            {
                currentPoint = pointA.transform;
            }
        }
    }
}
