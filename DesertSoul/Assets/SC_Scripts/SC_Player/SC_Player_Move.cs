using UnityEngine;
using UnityEngine.InputSystem;

//Handles player movement, sprite rotation, knockback, IFrames, and attacking
public class SC_Player_Move : MonoBehaviour
{

    private Vector2 horizontal;
    private float vertical;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 16f;
    private bool isFacingRight = true;
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float gravLimit = 10f;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    

    [SerializeField] private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private PlayerInputActions playerControls;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private InputAction move;
    [SerializeField] private InputAction look;

     private SC_Sythe sythe;
     private SC_Drill drill;

    //Knockback Vars
    [SerializeField] private float launchPower;
    [SerializeField] private float launchTime;
    [SerializeField] private float launchTotalTime;
    [SerializeField] private bool launchFromRight;

    //Iframe Vars
    [SerializeField] private float flickerSpeed;
    [SerializeField] private float flickerSpeedTotal;
    [SerializeField] private float Iframes;
    [SerializeField] private float IframeTotal;
    [SerializeField] private BoxCollider2D hitbox;

    private bool playerInControl = true;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        sythe = GetComponent<SC_Sythe>();
        drill = GetComponent<SC_Drill>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        Jump();
        horizontal = move.ReadValue<Vector2>();
        vertical = look.ReadValue<float>();
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
        if (playerInControl)
        {
            Move(horizontal, speed, true);
        }
    }

    public void SetCanMove(bool canMove)
    {
        playerInControl = canMove;
    }

    //made this public bc I forget how protected works
    //will change later
    public void Move(Vector2 movement, float speed, bool useGravity)
    {
        spriteRotation(movement);
        if (movement.x < 0)
        {
            anim.SetBool("isWalking", true);
        }
        else if(movement.x > 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        //if getting knocked back, player cannot move
        if (launchTime <= 0)
        {
            if (useGravity)
            {
                rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(movement.x * speed, movement.y * speed);
            }
        }
        else
        {
            //This system works for now, but doesent use gravity and feels floaty
            if(launchFromRight)
            {
                rb.linearVelocity = new Vector2(-launchPower, launchPower);
            }
            else
            {
                rb.linearVelocity = new Vector2(launchPower, launchPower);
            }

            launchTime -= Time.deltaTime;
        }

        //Iframes stuff
        if (Iframes > 0)
        {
            if (flickerSpeed > 0)
            {
                flickerSpeed -= Time.deltaTime;
            }
            else if(flickerSpeed <= 0)
            {
                sprite.enabled = !sprite.enabled;
                flickerSpeed = flickerSpeedTotal;
            }

            Iframes -= Time.deltaTime;
        }
        else
        {
            sprite.enabled = true;
            hitbox.enabled = true;
        }
    }

    private void spriteRotation(Vector2 movement)
    {
        if (movement.x < 0 && isFacingRight || movement.x > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    private void Jump()
    {

        //coyote time code
        if(IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //short hop code
        //temp(?)
        if (Input.GetButtonUp("Jump") && !IsGrounded())
        {
            rb.linearVelocity = new(rb.linearVelocity.x, -(jumpPower * 0.005f));
        }

        //Gets player input and jumps if grounded
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new(rb.linearVelocity.x, jumpPower);
            coyoteTimeCounter = 0f;
        }

        if(Input.GetButtonDown("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        //makes player decend faster the longer they are falling
        if(rb.linearVelocity.y < 0 && rb.gravityScale < gravLimit)
        {
            rb.gravityScale = rb.gravityScale + 0.005f;
        }

        //sets player gravity back to normal after being grounded
        if(IsGrounded())
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("PrimaryAttack"))
        {
            sythe.AddAttack(AttackType.primary);

        }

        if (Input.GetButtonDown("SecondaryAttack"))
        {
            drill.Drill(true, isFacingRight);
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Knockback(GameObject enemy)
    {

        //method 1
        //Vector3 dirVect = gameObject.transform.position - enemy.transform.position;
        //dirVect.Normalize();
        //rb.AddForce(dirVect * launchPower, ForceMode2D.Impulse);
        //rb.linearVelocity = dirVect * launchPower;

        //method 2
        launchTime = launchTotalTime;

        if (enemy.transform.position.x >= transform.position.x)
        {
            launchFromRight = true;
        }
        else if(enemy.transform.position.x < transform.position.x)
        {
            launchFromRight = false;
        }
    }

    public void SetIFrames()
    {
        Iframes = IframeTotal;
        flickerSpeed = flickerSpeedTotal;
        hitbox.enabled = false;
    }
}