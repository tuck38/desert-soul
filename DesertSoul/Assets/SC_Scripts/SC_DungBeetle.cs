using UnityEngine;
using System.Collections;

public class SC_DungBeetle : MonoBehaviour
{
    [SerializeField] int MAXhp;
    [SerializeField] Transform createPoint;
    int currHP;
    bool ballMade = false;
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

        if (!ballMade){
            StartCoroutine(createBall());
            ballMade = true;
        }
    }

    IEnumerator createBall()
    {
        Instantiate(dungBall, createPoint, createPoint);
        yield return new WaitForSeconds(2f);
        ballMade = false;
        
    }
}
