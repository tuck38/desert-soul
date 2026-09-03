using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class SC_BossBase : MonoBehaviour
{

    //TODO

   //ANIMATOR SWITCH, cancel attacks - done

   //stun time getter - done

   //hitbox hook up - done

   //parry player feedback

    [SerializeField] protected float MAXHealth;
    protected float currentHealth;

    [SerializeField] protected int phaseTransThreshold = 100;
    [SerializeField] protected int damage;
    [SerializeField] Animator bossAnim;
    [SerializeField] Transform beetleSpawn;
    [SerializeField] GameObject beetle;
    private bool phaseTransed = false;
     [SerializeField] private Transform groundCheck;
    private GameObject player;

    [SerializeField] GameObject healthBar;
    private Slider slider;

    [SerializeField] SpriteRenderer sprite;

    [SerializeField]LayerMask groundLayer;

    [SerializeField] Color baseColor;

    [SerializeField] AudioClip bossTheme;

    [SerializeField] Transform actualTransform;

    [SerializeField] Transform lastPlayerPos;

    public float halfWidth;

    public float halfHeight;

    private Rigidbody2D rb;
    private bool isFacingRight = true;

    [SerializeField] Color hurtColor;

    [SerializeField] float colorTime = 0.1f;

    float currentTime;

    [SerializeField] AudioClip hurtSound;

    [SerializeField] AudioClip bossDeath;

    bool lerping = false;

    bool canMove = true;

    [SerializeField] float parryStunTime = 0.5f;

    [SerializeField] float breakStunTime = 3f;

    float stunTime = 0.5f;

    float currentStunTime;

    float launchTime = 0;

    [SerializeField] float totalLaunchTime = 1f;

    float knockbackDist = 1f;

    bool launchFromRight;

    Vector3 kbEndPoint;

    [SerializeField] float totalLerpTime;

    float elapsedLerpTime;

    private bool parryable = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        currentTime = colorTime;
        currentHealth = MAXHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        slider = healthBar.GetComponent<Slider>();
        baseColor = sprite.color;
        halfHeight = sprite.bounds.extents.y;
        halfWidth = sprite.bounds.extents.x;
    }

    // Update is called once per frame
    protected void Update()
    {
        //I am so tired and want to go to sleep
        /*if(transform.position.x > player.position.x && isFacingRight)
        {
             
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
        else if(transform.position.x < player.position.x && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }*/


        if(currentTime < colorTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            ChangeColor(baseColor, true);
        }

        if(lerping)
        {

            if(Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall
                lerping = false;
                //SET CAN MOVE
                bossAnim.SetBool("doStun", false);
            }

            if(Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                //We are hitting Le wall
                lerping = false;
                //SET CAN MOVE
                bossAnim.SetBool("doStun", false);
            }


            elapsedLerpTime += Time.deltaTime;
            float percentageComplete = elapsedLerpTime / totalLerpTime;

            transform.position = Vector3.Lerp(transform.position, kbEndPoint, percentageComplete);

            //if statment of doom and dispair
            if(transform.position == kbEndPoint)
            {
                lerping = false;
                //set can move
                bossAnim.SetBool("doStun", false);
            }
        }

    }

    public int GetDamage()
    {
        return damage;
    }

    public bool GetDirection()
    {
        if(transform.position.x > player.transform.position.x)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public float GetStunTime()
    {
        return stunTime;
    }

    public void flipBoss()
    {
        if (transform.position.x > player.transform.position.x && isFacingRight || transform.position.x < player.transform.position.x && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));

        }
    }

    public Transform GetPlayerPos()
    {
        return player.transform;
    }

    public Transform GetLastPlayePos()
    {
        return lastPlayerPos;
    }

    public void SetLastPlayerPos()
    {
        lastPlayerPos = player.transform;
    }

    //takes damage and returns true if the attack killed the enemy
    public bool TakeBossDamage(SC_Attack_Base attack)
    {
        currentHealth -= attack.getDamage();
        slider.value = currentHealth / MAXHealth;
        ChangeColor(hurtColor, false);
        currentTime = 0;
        GameManager.Instance.playSFX(hurtSound.name, true);
        if (currentHealth <= phaseTransThreshold && phaseTransed == false)
        {
            GameObject obj = Instantiate(beetle, beetleSpawn.position, beetleSpawn.rotation);
            phaseTransed = true;
        }
        if (currentHealth <= 1)
        {
            //Debug.Log("Works");
            SceneManager.LoadScene("DemoOverScene");
            StartCoroutine(Die());
            return true;
        }
        return false;
    }

    public void StartFight()
    {
        bossAnim.SetBool("FightStart", true);
    }

    public Animator GetAnimator()
    {
        return bossAnim;
    }

    public void SetParryable(bool parry)
    {
        parryable = parry;
    }

    public void enableHPBar(bool enable)
    {
        //healthBar.SetActive(enable);
        //slider.value = 1;
    }

    public void BossMusic()
    {
        GameManager.Instance.playSong(bossTheme.name);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public Transform GetBossTransform()
    {
        return actualTransform;
    }

    public void Knockback(AttackType atkType, float kbDist)
    {
        if(atkType == AttackType.drillSide)
        {
            launchTime = totalLaunchTime;
        }

        knockbackDist = kbDist;

        Vector3 target = transform.position;

        if (player.transform.position.x >= transform.position.x)
        {
            launchFromRight = true;
            target = new Vector3(transform.position.x - knockbackDist, transform.position.y, transform.position.z);
        }
        else if (player.transform.position.x < transform.position.x)
        {
            launchFromRight = false;
            target = new Vector3(transform.position.x + knockbackDist, transform.position.y, transform.position.z);
        }

        if(atkType == AttackType.primary)
        {
                lerping = true;
                kbEndPoint = target;

                //Cancel current attack
        }
    }


    public void ChangeColor(Color color, bool originalColor)
    {
        if(originalColor)
        {
            sprite.color = baseColor;
        }
        else
        {
            sprite.color = color;
        }
    }

    public bool Parried(AttackType atk, float kbDist)
    {
        if(parryable)
        {
            Knockback(atk, kbDist);
            stunTime = parryStunTime;
            bossAnim.SetBool("doStun", true);
            bossAnim.SetBool("doAttack1", false);
            bossAnim.SetBool("doAttack2", false);
        }
        return parryable;
    }

    IEnumerator Die()
    {
        GameManager.Instance.playSFX(bossDeath.name, true);
        yield return new WaitForSeconds(0.2f);
        Destroy(this);
    }

}
