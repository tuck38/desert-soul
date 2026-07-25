using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

//Handles player movement, sprite rotation, knockback, IFrames, and attacking
//Was lazy and ended up putting menu inputs here aswell, might move them to another script at some point, 
//Right now they live in "other inputs"
public class SC_Player_Move : MonoBehaviour
{
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

    [SerializeField] UnityEngine.UI.Image fade;

    //lets me save this for the knockback calc
    Vector2 moveVector;

    //Jump Variables
    [Tooltip("How far the player can go up before they fall if the jump button is held")]
    [SerializeField] private float maxJumpHeight = 8f;
    [Tooltip("How mfar the player must go up before they fall if the jump button is released during the jump")]
    [SerializeField] private float minJumpHeight = 2f;
    [Tooltip("How fast the player will go up upon the jump button being pressed")]
    [SerializeField] private float jumpVelocity = 8f;
    [SerializeField] private float airMoveMult = 0.8f;
    private float lastYValue;

    //vars the keep track of jump state
    private float currentJumpHeight = 0f;
    private float startJumpHeight = 0f;
    private bool falling = false;
    private bool stopJump = false;

    //Gravity
    [Tooltip("The base gravity acting upon the player, reset after they are grounded")]
    [SerializeField] private float baseGravity;
    //CameraTracker
    [SerializeField] Transform cameraUp;
    [SerializeField] Transform cameraDown;
    //fastfall
    [Tooltip("The factor of how much the gravity increases on the player as they are falling")]
    //
    [SerializeField] private float fallGravModifier;
    //Gravity is multiplied by this when the player begins to fall or releases the jump key
    [SerializeField] private float fallGravMult;
    [Tooltip("How fast the player will go up upon the jump button being pressed")]
    [SerializeField] private float fallingGravLimit = 10f;

    [Tooltip("The time the player has to jump after leaving an edge")]
    [SerializeField] private float coyoteTime = 0.2f;

    private float coyoteTimeCounter;

    //peak jump vars

    [SerializeField] float peakJumpClamp;

    [SerializeField] float peakJumpGravMult = 0.5f;

    [SerializeField] float velocityPeakCut = 0.4f;

    [SerializeField] float velocityReleaseCut = 0.5f;

    bool peak = false;

    float prePeakJumpGrav;

    public bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] public PlayerInputActions playerControls;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private InputAction move;
    [SerializeField] private InputAction look;
     [SerializeField] private InputAction jump;
    [SerializeField] private InputAction journal;
    [SerializeField] private InputAction journalTabs;
    [SerializeField] private InputAction interact;

    [SerializeField] AudioClip[] sytheSwingAUD;
    [SerializeField] AudioClip[] movementAUD;

    [SerializeField] float stepSoundFrequency;
    private float currentStepSound = 0f;
    [SerializeField] AudioClip jumpAUD;

    //Not implemented
    [SerializeField] AudioClip landAUD;


     private SC_Sythe sythe;
     private SC_Drill drill;

    //Knockback Vars
    [SerializeField] private float launchPower;
    private float launchTime;
    [SerializeField] private float launchTotalTime;
    [SerializeField] private bool launchFromRight;

    [SerializeField] public bool knockbackActive = false;

    //Iframe Vars
    private float flickerSpeed = 0;
    [SerializeField] private float flickerSpeedTotal;
    private float Iframes = 0;
    [SerializeField] private float IframeTotal;
    [SerializeField] private BoxCollider2D hitbox;

    [SerializeField] SC_Player_Prop prop;

    //Camera Vars
    [SerializeField] private GameObject cameraFollowGameObject;
    private SC_Camera_FollowObject cameraFollowObject;

    int kbX = 0;
    //UI
    [SerializeField] SC_Journal Journal;
    [SerializeField] Text stoneAmount;

    bool InUI;

    private bool playerInControl;

    private bool jumping;

    private bool jumpUp = false;
    //kick back whip around and spin

    private bool drillGot = false;

    private Vector3 spawnPos;

    private bool interacting = false;

    private bool usingGravity = true;

    //TEMP 

    [SerializeField] float fadeTime;

    float currentFadeTime = 0;

    bool fadeIn = false;

    public bool iFramesActive = false;

    bool fadeOut = false;

    [SerializeField] bool firstRoom = false;

    bool paused = false;

    private void Awake()
    {
        //initial decloration
        playerControls = new PlayerInputActions();

        //look function
        playerControls.Player.Look.performed += onLook;
        playerControls.Player.Look.canceled += onLook;

        //binding methods
        playerControls.Player.Jump.performed += OnJump;
        playerControls.Player.Jump.canceled += OnJump;
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;

        //Attacks
        playerControls.Player.PrimaryAttack.performed += OnPrimary;
        playerControls.Player.SecondaryAttack.performed += OnSecondary;

        //UI
        playerControls.Player.Interact.performed += OnInteract;
        playerControls.Player.Interact.canceled += OnInteract;
        playerControls.Player.Journal.performed += OnMenu;
        playerControls.Player.JournalLeft.performed += OnJournalTabLeft;
        playerControls.Player.JournalRight.performed += OnJournalTabRight;
        playerControls.Player.Blueprint.performed += OnTownBlueprint;
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
        
        //SetFade();

        playerInControl = true;

        paused = GameManager.Instance.gameIsPaused;

        if(firstRoom)
        {
            spawnPos = transform.position;
            playerInControl = false;
        }

        flickerSpeed = flickerSpeedTotal;
        currentSpeed = minSpeed;
        currentMoveAccelerationTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(firstRoom)
        {
            transform.position = spawnPos;
        }

        //inst used rn, havent implemented camrea looking
        vertical = look.ReadValue<float>();

        MoveUpdate(moveVector, currentSpeed, usingGravity);

        if(knockbackActive)
        {
            KnockbackUpdate();
        }

        if(moving && IsGrounded() && !drill.GetDrillin())
        {
            StepSound();
        }
        IFramesUpdate();


        //fade in da update func
        if(fadeIn)
        {
            if(fadeTime >= currentFadeTime)
            {
                currentFadeTime += Time.deltaTime;
                float percentegeComplete = currentFadeTime/ fadeTime;

                float currentFade = Mathf.Lerp(1, 0, percentegeComplete);

                fade.color = new UnityEngine.Color(fade.color.r, fade.color.g, fade.color.b, currentFade);
            }
            else
            {
                currentFadeTime = 0;
                fadeIn = false;
                //fadeOut = false; 
                //SceneManager.LoadScene(LastScene);
            }
        }
        else if (fadeOut)
        {
            if(fadeTime >= currentFadeTime)
            {
                currentFadeTime += Time.deltaTime;
                float percentegeComplete = currentFadeTime/ fadeTime;

                float currentFade = Mathf.Lerp(0, 1, percentegeComplete);

                fade.color = new UnityEngine.Color(fade.color.r, fade.color.g, fade.color.b, currentFade);
            }
            else
            {
                currentFadeTime = 0;
                fadeOut = false;
                //fadeOut = false; 
                //SceneManager.LoadScene(LastScene);
            }
        }
    }

    private void OnEnable()
    {
        SC_Shop.OnToggleShop += SetCanMove;
        playerControls.Enable();
        move = playerControls.Player.Move;
        move.Enable();
        look.Enable();
    }

    private void OnDisable()
    {
        SC_Shop.OnToggleShop -= SetCanMove;
        playerControls.Disable();
        move.Disable();
        look.Disable();
    }

    private void FixedUpdate()
    {
        if (playerInControl)
        {
            Gravity();
        }
    }

    //TEMP

    public void isFirstRoom(bool room)
    {
        firstRoom = room;
    }

    public UnityEngine.UI.Image getFade()
    {
        return fade;
    }

    public void SetFade()
    {
        fade.color = Color.black;
    }

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }

    public void SetCanMove(bool canMove)
    {
        playerInControl = canMove;
        Move(Vector2.zero, 0, true);
    }

    public void SetPaused()
    {
        paused = GameManager.Instance.gameIsPaused;
    }

    public void onLook(InputAction.CallbackContext context)
    {
        Vector2 lookDirection = context.ReadValue<Vector2>();
        //Debug.Log("InputGotten");
        if (context.performed){
            if (lookDirection.y > 0)
            {
                cameraFollowGameObject.GetComponent<SC_Camera_FollowObject>().lookingUp(cameraUp);
                Debug.Log("LookingUp");
            }
            else if(lookDirection.y < 0)
            {
                cameraFollowGameObject.GetComponent<SC_Camera_FollowObject>().lookingDown(cameraDown);
                Debug.Log("LookingDown");
            }
        }
        else if (context.canceled)
        {
            cameraFollowObject.notLooking();
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        //while this function is called when move inputs are read, movement calculation is still handled in the move function
        //due to outside sources calling it when player must be moved (drill)
        if(playerInControl && paused == false)
        {
            Move(context.ReadValue<Vector2>(), currentSpeed, true);
        }
        else if(GameManager.Instance.GetBuildMode())
        {
            GameManager.Instance.BuildMove(context.ReadValue<Vector2>());
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(playerInControl && paused == false && !GameManager.Instance.GetBuildMode())
        {
        if(context.performed)
        {
            /*if(rb.linearVelocity.y > 0)
            {
                SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }*/

            //Gets player input and jumps if grounded
            if ((IsGrounded() || coyoteTimeCounter > 0f) && !jumping)
            {
                anim.SetBool("isJumping", true); 
                GameManager.Instance.playSFX(jumpAUD.name);
                stopJump = false;
                jumping = true;
                startJumpHeight = gameObject.transform.position.y;
                lastYValue = gameObject.transform.position.y;
                SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
                coyoteTimeCounter = 0f;
                //playerJump.Post(gameObject);
            }


        }
        else if(context.canceled)
        {
            if (!IsGrounded() && jumping == true)
            {
                SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
                rb.gravityScale = rb.gravityScale * fallGravMult;
                rb.linearVelocity = new(rb.linearVelocity.x, rb.linearVelocityY * velocityReleaseCut);
                stopJump = true;
            }
        }
        }
        else if (GameManager.Instance.GetBuildMode() && context.performed)
        {
            GameManager.Instance.Confirm();
        }
    }

    public void OnPrimary(InputAction.CallbackContext context)
    {
        if(paused == false)
        {
            SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
            sythe.AddAttack(AttackType.primary);
            GameManager.Instance.playSFX(sytheSwingAUD[0].name, true);
        }
    } 

    public void OnSecondary(InputAction.CallbackContext context)
    {
        if(drillGot && paused == false)
        {
            SC_DustCloud.OnPlayerTakeAnAction?.Invoke();
            drill.Drill(IsGrounded(), isFacingRight);
        }
    }

    public void setHasDrill(bool drill)
    {
        drillGot = drill;
    }

    //bad, fix, make all part of state machine
    public void DoDrill()
    {
        drill.Yeah();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(paused == false)
        {
            if(context.performed)
            {
                interacting = true;
            }

            if(context.canceled)
            {
                interacting = false;
            }
        }
    }

    //this is where the UIbinds start, unaware if I wish to switch this to a seperate script or not, we shall see
    public void OnMenu(InputAction.CallbackContext context)
    {
        if(context.performed && InUI == false && !GameManager.Instance.GetBuildMode())
        {
            InUI = true;
            GameManager.Instance.TogglePause();
            Journal.OpenJournal();
            prop.UpdateText();
            //journalOpen.Post(gameObject);

        }
        else if(context.performed && InUI == true)
        {
            InUI = false;
            GameManager.Instance.TogglePause();
            Journal.CloseJournal();
            //journalClose.Post(gameObject);
        }
    }

    //could make these 2 a pos/neg bind instead
    public void OnJournalTabLeft(InputAction.CallbackContext context)
    {
        if(InUI)
        {
        Journal.NewTab(true);
        }
    }

    public void OnJournalTabRight(InputAction.CallbackContext context)
    {
        if(InUI)
        {
        Journal.NewTab(false);
        }
    }

    //this is in another script, need to move it to this one
    public void OnTownBlueprint(InputAction.CallbackContext context)
    {
        if(!paused)
        {
            GameManager.Instance.ToggleBuildMode();
        }
    }

    //handles all movement calculations
    public void Move(Vector2 movement, float speed, bool useGravity)
    {
        usingGravity = useGravity;
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
        moveVector = movement;
    }

    private void StepSound()
    {
        if(stepSoundFrequency > currentStepSound)
        {
            currentStepSound += Time.deltaTime;
        }
        else if(stepSoundFrequency <= currentStepSound)
        {
            int num = Random.Range(0, movementAUD.Length);
            GameManager.Instance.playSFX(movementAUD[num].name);
            currentStepSound = 0;
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

    private void Gravity()
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

        //I dont remember what the fuck this is doing
        if (lastYValue.ToString("F2") == gameObject.transform.position.y.ToString("F2"))
        {
            if (!IsGrounded())
            {
                
                stopJump = true;
                jumping = false;
            }
        }
        lastYValue = gameObject.transform.position.y;
        //OH THIS IS SO THE PLAYER STOPS JUMPING IF THEY BUMP THEY HEAD

        if (jumping)
        {
            
            rb.linearVelocity = new(rb.linearVelocity.x, 1 * jumpVelocity);
        }

        //short hop
        if (transform.position.y - startJumpHeight >= minJumpHeight && stopJump)
        {
            jumpUp = true;
            jumping = false;
        }


        //near peak of jump
        if (transform.position.y - startJumpHeight > maxJumpHeight - peakJumpClamp && jumping == true && peak == false)
        {
            prePeakJumpGrav = rb.gravityScale;
            rb.gravityScale = rb.gravityScale * peakJumpGravMult;
            peak = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * velocityPeakCut);
        }

        //leaving peak of jump
        if(transform.position.y - startJumpHeight < maxJumpHeight - peakJumpClamp  && peak == true)
        {
            rb.gravityScale = prePeakJumpGrav;
            peak = false;
            rb.gravityScale = rb.gravityScale * fallGravMult;
        }

        //stop player after reachin max jump
        if (transform.position.y - startJumpHeight >= maxJumpHeight && jumping == true)
        {
            jumpUp = true;
            jumping = false;
        }

        //makes player decend faster the longer they are falling
        if(rb.linearVelocity.y <= 0 && rb.gravityScale < fallingGravLimit && peak == false)
        {
            rb.gravityScale = rb.gravityScale + fallGravModifier;
        }

        //sets player gravity back to normal after being grounded
        if(IsGrounded())
        {
            if(jumpUp)
            {
                anim.SetBool("isJumping", false);
                //GameManager.Instance.playSFX(landAUD.name);
                jumpUp = false;
            }
            stopJump = false;
            rb.gravityScale = baseGravity;
        }
    }


    //will make event in future
    //at some point
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
            kbX = -1;

        }
        else if(enemy.transform.position.x < transform.position.x)
        {
            launchFromRight = false;
            kbX = 1;
        }

        knockbackActive = true;
    }

    private void MoveUpdate(Vector2 movement, float speed, bool useGravity)
    {
        //if getting knocked back, player cannot move
        if (launchTime <= 0)
        {
            if (useGravity && !IsGrounded())
            {
                rb.linearVelocity = new Vector2(movement.x * (speed * airMoveMult), rb.linearVelocity.y);
            }
            else if (useGravity)
            {
                rb.linearVelocity = new Vector2(movement.x * (speed), rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(movement.x * speed, movement.y * speed);
            }
        }
    }

    private void KnockbackUpdate()
    {
        if(launchTime > 0)
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
        else
        {
            knockbackActive = false;
            Vector2 zer = Vector2.zero;
            rb.linearVelocity = zer;
        }
    }

    public void SetIFrames()
    {
        Iframes = IframeTotal;
        flickerSpeed = flickerSpeedTotal;
        iFramesActive = true;
        hitbox.enabled = false;
    }

    private void IFramesUpdate()
    {
        if(iFramesActive == true)
        {
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

                hitbox.enabled = false;
                Iframes -= Time.deltaTime;
            }
            else
            {
                iFramesActive = false;
                sprite.enabled = true;
                hitbox.enabled = true;
            }
        }
    }
}