using UnityEngine;

public class SC_Dungball : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] Vector3 maxScale;
    [SerializeField] float damage;
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
        else parent.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        parent.localScale = Vector3.Lerp(parent.localScale, maxScale, timeElapsed / timeToFullyFormed);
        isFullyFormed = timeElapsed / timeToFullyFormed >= 1;
    }

    public bool IsDungballFullyFormed()
    {
        return isFullyFormed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(startedFall)
        {
            finalFallYPos = parent.position.y;
            if (initialFallYPos - finalFallYPos > maxFallHeight) Destroy(gameObject);
            startedFall = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((LayerMask)collision.gameObject.layer == groundLayer)
        {
            initialFallYPos = parent.position.y;
            startedFall = true;
        }
    }
}
