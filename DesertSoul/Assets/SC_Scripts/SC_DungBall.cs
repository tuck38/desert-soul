using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class SC_DungBall : MonoBehaviour
{

    public float ballRollSPD;
    [SerializeField] Transform ballSprite;
    public bool ballThrown = false;
    public bool isFacingRight;
    public int damageDealt;
    private Rigidbody2D rb;
    SC_DungBeetle dungbeetleScript;
    public void GetParentBeetle(SC_DungBeetle parent)
    {
        dungbeetleScript = parent;
    }

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //SC_DungBeetle dungbeetleScript = GetComponentInParent<SC_DungBeetle>();
        StartCoroutine(rollBall());
        StartCoroutine(breakBalls());
    }

    IEnumerator rollBall()
    {
        yield return new WaitForSeconds(1f);

        ballThrown = true;
    }

    IEnumerator breakBalls()
    {
        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }

    IEnumerator resetBall()
    {
        yield return new WaitForSeconds(0.2f);
        ballThrown = false;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (ballThrown) {
            if(dungbeetleScript.isFacingLeft == false){
                rb.AddForce(transform.right * ballRollSPD);
            }
            else
            {
                rb.AddForce(transform.right * (ballRollSPD*-1));
            }
            rb.AddForce(transform.up * ballRollSPD);
            StartCoroutine(resetBall());
        }
    }

    public void Break()
    {
        Destroy(gameObject);
    }

    public int GetDamage()
    {
        return damageDealt;
    }
}
