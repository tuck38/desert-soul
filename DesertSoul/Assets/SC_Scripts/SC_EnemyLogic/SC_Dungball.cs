using UnityEngine;

public class SC_Dungball : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] Vector3 maxScale;
    [SerializeField] int damage;
    [SerializeField] float timeToFullyFormed;
    [SerializeField] float maxFallHeight;
    [SerializeField] bool isFullyFormed;
    [SerializeField] LayerMask groundLayer;

    float timeElapsed = 0.0f;
    bool startedFall = false;
    float initialFallYPos, finalFallYPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isFullyFormed) parent.localScale = maxScale;
        else parent.localScale = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFullyFormed)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / timeToFullyFormed;
            parent.localScale = Vector2.Lerp(Vector2.zero, maxScale, t);
            isFullyFormed = t >= 1;
            //Debug.Log($"TimeToFullyForm {timeToFullyFormed}, Time Elasped {timeElapsed}, t value {t}, Scale {parent.localScale}");
        }
    }

    public bool IsDungballFullyFormed()
    {
        return isFullyFormed;
    }

    public int GetDamage()
    {
        return damage;
    }

    public bool IsFullyFormed()
    {
        return isFullyFormed;
    }

    public void DamageBall()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("End Fall?");
        if (startedFall)
        {
            //Debug.Log("End Fall!");
            finalFallYPos = transform.position.y;
            if (initialFallYPos - finalFallYPos > maxFallHeight) Destroy(gameObject);
            startedFall = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Start Fall?");
        if (!startedFall && ((groundLayer.value & (1 << collision.gameObject.layer)) != 0))
        {
            //Debug.Log("Start Fall!");
            initialFallYPos = transform.position.y;
            startedFall = true;
        }
    }
}
