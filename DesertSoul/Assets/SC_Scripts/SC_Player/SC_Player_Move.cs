using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Handles player movement, sprite rotation, knockback, IFrames, and attacking
public class SC_Player_Move : MonoBehaviour
{

    private Vector2 horizontal;
    private float vertical;

    //Move Variables
    [Tooltip("How much the players speed increases every x seconds, up to the max")]
    [SerializeField] private float moveAcceleration = 1f;
    [Tooltip("How many seconds before move acceleration is applied to the player")]
    [SerializeField] private float moveAccelerationTimer = 0.2f;
    private float currentMoveAccelerationTimer;
    [Tooltip("The maximum speed the player can move")]
    [SerializeField] private float maxSpeed = 8f;
    [Tooltip("How fast the player moves at base, before acceleration")]
    [SerializeField] private float minSpeed = 4f;
    private float currentSpeed;
    bool moving = false;

    //Jump Variables
    [Tooltip("How far the player can go up before they fall if the jump button is held")]
    [SerializeField] private float maxJumpHeight = 8f;
    [Tooltip("How mfar the player must go up before they fall if the jump button is released during the jump")]
    [SerializeField] private float minJumpHeight = 2f;
    [Tooltip("How fast the player will go up upon the jump button being pressed")]
    [SerializeField] private float jumpVelocity = 8f;
    private float lastYValue;

    //vars the keep track of jump state
    private float currentJumpHeight = 0f;
    private float startJumpHeight = 0f;
    private bool falling = false;
    private bool stopJump = false;

    //Gravity
    [Tooltip("The base gravity acting upon the player, reset after they are grounded")]
    [SerializeField] private float baseGravity = 2f;

    //fastfall
    [Tooltip("The factor of how much the gravity increases on the player as they are falling")]
    [SerializeField] private float gravModifier = 0.005f;
    [Tooltip("How fast the player will go up upon the jump button being pressed")]
    [SerializeField] private float fallingGravLimit = 10f;

    [Tooltip("The time the player has to jump after leaving an edge")]
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public bool isFacingRight = true;

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
    private float launchTime;
    [SerializeField] private float launchTotalTime;
    [SerializeField] private bool launchFromRight;

    //Iframe Vars
    private float flickerSpeed = 0;
    [SerializeField] private float flickerSpeedTotal;
    private float Iframes = 0;
    [SerializeField] private float IframeTotal;
    [SerializeField] private BoxCollider2D hitbox;

    //Camera Vars
    [SerializeField] private GameObject cameraFollowGameObject;
    private SC_Camera_FollowObject cameraFollowObject;

    //UI
    [SerializeField] SC_Journal Journal;
    [SerializeField] Text stoneAmount;

    bool InUI;

    private bool playerInControl;

    private bool jumping;

    private bool interacting = false;

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

        cameraFollowObject = cameraFollowGameObject.GetComponent<SC_Camera_FollowObject>();

        jumping = false;
        playerInControl = true;

        flickerSpeed = flickerSpeedTotal;
        currentSpeed = minSpeed;
        currentMoveAccelerationTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        OtherInputs();
        horizontal = move.ReadValue<Vector2>();
        vertical = look.ReadValue<float>();
    }

    private void OnEnable()
    {
        SC_Shop.OnToggleShop += SetCanMove;
        move = playerControls.Player.Move;
        move.Enable();
        look.Enable();
    }

    private void OnDisable()
    {
        SC_Shop.OnToggleShop -= SetCanMove;
        move.Disable();
        look.Disable();
    }

    private void FixedUpdate()
    {
        if (playerInControl)
        {
            Jump();
            Move(horizontal, currentSpeed, true);
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
            SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
            moving = true;
            anim.SetBool("isWalking", true);
        }
        else if(movement.x > 0)
        {
            SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
            moving = true;
            anim.SetBool("isWalking", true);
        }
        else
        {
            moving = false;
            currentMoveAccelerationTimer = 0;
            currentSpeed = minSpeed;
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

        if (currentSpeed < maxSpeed && moving == true)
        {
            if(currentMoveAccelerationTimer < moveAccelerationTimer)
            {
                currentMoveAccelerationTimer += Time.deltaTime;
            }
            else
            {
                currentSpeed += moveAcceleration;
                if(currentSpeed > maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
                currentMoveAccelerationTimer = 0;
            }
        }
    }

    private void spriteRotation(Vector2 movement)
    {
        if (movement.x < 0 && isFacingRight || movement.x > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));

            cameraFollowObject.Turn();
        }
    }

    private void OtherInputs()
    {

        if (!Input.GetButton("Jump") && !IsGrounded() && jumping == true)
        {
            SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
            //rb.linearVelocity = new(rb.linearVelocity.x, -(jumpPower * 0.005f));
            stopJump = true;
        }

        //Gets player input and jumps if grounded
        if (Input.GetButtonDown("Jump") && (IsGrounded() || coyoteTimeCounter > 0f) && !jumping)
        {
            stopJump = false;
            jumping = true;
            startJumpHeight = gameObject.transform.position.y;
            lastYValue = gameObject.transform.position.y;
            SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
            coyoteTimeCounter = 0f;
        }

        if(Input.GetButtonDown("Interact"))
        {
            interacting = true;
        }

        if(Input.GetButtonUp("Interact"))
        {
            interacting = false;
        }

        if(Input.GetButtonDown("MenuIn") && InUI == false)
        {
            InUI = true;
            Journal.OpenJournal();

        }

        if(Input.GetButtonDown("MenuOut") && InUI == true)
        {
            InUI = false;
            Journal.CloseJournal();
        }

        if(Input.GetButtonDown("TabLeft") && InUI == true)
        {
            Journal.NewTab(true);
        }

        if(Input.GetButtonDown("TabRight") && InUI == true)
        {
            Journal.NewTab(false);
        }
    }

    private void Jump()
    {
        //coyote time code
        if (IsGrounded() && !jumping)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (lastYValue.ToString("F2") == gameObject.transform.position.y.ToString("F2"))
        {
            if (!IsGrounded())
            {
                
                stopJump = true;
                jumping = false;
            }
        }
        lastYValue = gameObject.transform.position.y;


        if (jumping)
        {
            rb.linearVelocity = new(rb.linearVelocity.x, 1 * jumpVelocity);
        }

        if (transform.position.y - startJumpHeight >= minJumpHeight && stopJump)
        {
            jumping = false;
        }

        if (transform.position.y - startJumpHeight >= maxJumpHeight)
        {
            jumping = false;
        }

        if(Input.GetButtonDown("Jump") && rb.linearVelocity.y > 0f)
        {
            SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        //makes player decend faster the longer they are falling
        if(rb.linearVelocity.y < 0 && rb.gravityScale < fallingGravLimit)
        {
            rb.gravityScale = rb.gravityScale + gravModifier;
        }

        //sets player gravity back to normal after being grounded
        if(IsGrounded())
        {
            stopJump = false;
            rb.gravityScale = baseGravity;
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("PrimaryAttack"))
        {
            SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
            sythe.AddAttack(AttackType.primary);

        }

        if (Input.GetButtonDown("SecondaryAttack"))
        {
            SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
            drill.Drill(IsGrounded(), isFacingRight);
        }
    }


    //will make event in future
    public bool IsInteracting()
    {
        return interacting;
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