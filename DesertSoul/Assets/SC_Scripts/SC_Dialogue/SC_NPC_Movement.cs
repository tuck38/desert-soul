using UnityEngine;

public class SC_NPC_Movement : MonoBehaviour
{

    public Vector2 movementTarget;

    public float speed = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movementTarget != null)
        {
            Debug.Log("Target position: " + movementTarget + " | Current Position: " + transform.position);
            transform.position = Vector2.MoveTowards(transform.position, movementTarget, speed * Time.deltaTime);
        }
    }
}
