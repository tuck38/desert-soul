using UnityEngine;

public class KnatAI : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed;

    [SerializeField] Transform point1;
    [SerializeField] Transform point2;
    bool isGoingOne;
    private Transform nextPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPoint = point1;
        isGoingOne = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPos();
        MoveTo();
    }

    private void CheckPos()
    {
        float dist = Vector2.Distance(transform.position, nextPoint.position);
        if(dist < 0.5)
        {
            if(isGoingOne)
            {
                nextPoint = point2;
                isGoingOne = false;
            }
            else 
            {
                nextPoint = point1;
                isGoingOne = true;
            }
        }
    }

    private void MoveTo()
    {
        float dist = Vector2.Distance(transform.position, nextPoint.position);
        Vector2 dir = nextPoint.position - transform.position;

        transform.position = Vector2.MoveTowards(this.transform.position, nextPoint.position, speed * Time.deltaTime);
    }
}
