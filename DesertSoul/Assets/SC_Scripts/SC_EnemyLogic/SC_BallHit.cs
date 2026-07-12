using UnityEngine;

public class SC_BallHit : MonoBehaviour
{
    [SerializeField] GameObject Ball;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            Destroy(Ball);
        }
        if(col.gameObject.tag == "Dungball")
        {
            Destroy(Ball);
        }
    }
}
