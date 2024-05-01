using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 horizontal;
    private float vertical;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 16f;
    [SerializeField] private float launchPower = 10f;
    private bool isFacingRight = true;
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float gravLimit = 10f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private PlayerInputActions playerControls;
    [SerializeField] private InputAction move;
    [SerializeField] private InputAction look;
    [SerializeField]private InputAction swing;

    private Attack weapon;

    private void Awake()
    {
          playerControls = new PlayerInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        weapon = gameObject.GetComponentInChildren<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Flip();
        horizontal = move.ReadValue<Vector2>();
        vertical = look.ReadValue<float>();
        weapon.setSwingVert(vertical);
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        look.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Flip()
    {
        if (isFacingRight && horizontal.x < 0f || !isFacingRight && horizontal.x > 0f)
        {
            if (!weapon.getAttacking())
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }

    //ground check :3
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //read the function
    private void Move()
    {
        rb.velocity = new Vector2(horizontal.x * speed, rb.velocity.y);
    }

    //juming!! Yippee!!
    private void Jump()
    {
        //gets player input and jumps if they are grounded
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) 
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        //makes player decend faster the longer they are falling
        if (rb.velocity.y < 0 && rb.gravityScale < gravLimit)
        {
            rb.gravityScale = rb.gravityScale + 0.005f;
        }

        //sets the players gravity back to normal after they reach the ground
        if (IsGrounded())
        {
            rb.gravityScale = baseGravity;
        }

        //meant to gve more hang time at the peak of jump, not working atm
        /*if (!IsGrounded() && Mathf.Abs(rb.velocity.y) < 0.1)
        {
            rb.gravityScale = baseGravity * 0.5f;
        }*/
    }

    //Launches player upwards in the event of a downward strike
    public void Launch()
    {
        rb.velocity = new Vector2(rb.velocity.x, launchPower);
    }
}
