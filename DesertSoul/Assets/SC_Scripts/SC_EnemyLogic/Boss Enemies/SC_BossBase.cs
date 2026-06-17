using UnityEngine;

public class SC_BossBase : MonoBehaviour
{

    private Transform player;

    private bool isFacingRight = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void flipBoss()
    {
        if (transform.position.x > player.position.x && isFacingRight || transform.position.x < player.position.x && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));

        }
    }
}
