using UnityEngine;
using System.Collections;

public class SC_DungBeetle : MonoBehaviour
{
    [SerializeField] int MAXhp;
    [SerializeField] Transform createPoint;
    int currHP;
    [SerializeField] private Animator animator;
    [SerializeField] GameObject dungBall;
    SC_DungBall ballCode;

    void Start()
    {
        currHP = MAXhp;
    }

    void Update()
    {
        if (currHP <= 0)
        {
            Destroy(this);
        }

        StartCoroutine(createBall());
    }

    IEnumerator createBall()
    {
        
        yield return new WaitForSeconds(2f);

        Instantiate(dungBall, createPoint, createPoint);
    }
}
