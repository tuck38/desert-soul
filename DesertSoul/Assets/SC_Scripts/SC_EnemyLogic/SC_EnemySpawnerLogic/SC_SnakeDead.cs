using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SC_SnakeDead : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float launchPower = 5f;

    [SerializeField] float totalLaunchTime = 1f;

    [SerializeField] GameObject actuallyDead;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] Transform groundCheck;

    float launchTime = 0f;
    private bool kB = false;

    private bool right;

    private bool destroy = false;

    private Vector3 lastPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(launchTime > 0)
        {
            if(right)
            {
                rb.linearVelocity = new Vector2(-launchPower, launchPower);
            }
            else
            {
                rb.linearVelocity = new Vector2(launchPower, launchPower);
            }
            launchTime -= Time.deltaTime;
        }
        else if(launchTime < 0 && kB == true)
        {
            destroy = true;
        }

        if(Physics2D.OverlapCircle(groundCheck.position, 0.01f, groundLayer) && destroy == true)
        {
            Instantiate(actuallyDead, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

    public void knockBack(bool dirRight)
    {
        launchTime = totalLaunchTime;
        kB = true;
        right = dirRight;
    }
}
