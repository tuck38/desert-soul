using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SC_DustCloud : MonoBehaviour
{
    public static Action OnPlayerTakeAnAction;

    [SerializeField] Image dustBlindess;
    [SerializeField] float timeForDustBlindnessToVanish;
    [SerializeField] float amountOfDustRemovedPerAction;
    [SerializeField] float speed;
    [SerializeField] Transform[] pathPoints;
    [SerializeField] bool useTimeBasedDustRemover;

    bool removeDust = false, isMovingForward = true;
    float timeElaspedForRemovingDust = 0.0f;
    int currentIndex = 0;
    private Transform nextPoint;

    private void OnEnable()
    {
        OnPlayerTakeAnAction += UpdateDustOpacity; 
    }

    private void OnDisable()
    {
        OnPlayerTakeAnAction -= UpdateDustOpacity;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dustBlindess.color = new Color(dustBlindess.color.r, dustBlindess.color.g, dustBlindess.color.b, 0);
        nextPoint = pathPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        MoveTo();
        CheckPos();
        if (removeDust && useTimeBasedDustRemover && timeElaspedForRemovingDust < timeForDustBlindnessToVanish)
        {
            timeElaspedForRemovingDust += Time.deltaTime;
            float lerpTimeElapsed = Mathf.Lerp(1, 0, timeElaspedForRemovingDust / timeForDustBlindnessToVanish);
            dustBlindess.color = new Color(dustBlindess.color.r, dustBlindess.color.g, dustBlindess.color.b, lerpTimeElapsed);
        }
    }

    private void CheckPos()
    {
        float dist = Vector2.Distance(transform.position, nextPoint.position);
        if (dist < 0.5f)
        {
            if (isMovingForward && currentIndex + 1 == pathPoints.Length) isMovingForward = false;
            else if (!isMovingForward && currentIndex - 1 == -1) isMovingForward = true;
            
            if (isMovingForward)
            {
                currentIndex++;
                nextPoint = pathPoints[currentIndex];
            }
            else
            {
                currentIndex--;
                nextPoint = pathPoints[currentIndex];
            }
        }
    }

    private void MoveTo()
    {
        float initialY = transform.position.y;
        Vector2 newXPosition = Vector2.MoveTowards(transform.position, nextPoint.position, speed * Time.deltaTime);
        transform.position = new Vector2(newXPosition.x, initialY);
    }

    private void UpdateDustOpacity()
    {
        if(removeDust && !useTimeBasedDustRemover)
        {
            dustBlindess.color = new Color(dustBlindess.color.r, dustBlindess.color.g, dustBlindess.color.b, dustBlindess.color.a - amountOfDustRemovedPerAction);
            if (amountOfDustRemovedPerAction <= 0) removeDust = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Player") dustBlindess.color = new Color(dustBlindess.color.r, dustBlindess.color.g, dustBlindess.color.b, 1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            removeDust = true;
            timeElaspedForRemovingDust = 0f;
        }
    }
}
