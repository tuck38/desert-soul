using UnityEngine;
using System.Collections;

public class SC_DungBall : MonoBehaviour
{

    public float ballRollSPD;
    [SerializeField] Transform ballSprite;
    public bool ballThrown = false;
    public bool isFacingRight;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    IEnumerator rollBall()
    {
        yield return new WaitForSeconds(1.5f);

        ballThrown = true;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (ballThrown) {
            if (isFacingRight){
                //transform.Rotate(transform.forward * ballRollSPD * Time.deltaTime);
            }
            else
            {
                //transform.Rotate(transform.forward * -ballRollSPD * Time.deltaTime);
            }
            rb.AddForce(transform.right * ballRollSPD);
        }
    }
}
