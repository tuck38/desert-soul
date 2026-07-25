using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering;

public class SC_BossBase : MonoBehaviour
{

    [SerializeField] protected float MAXHealth;
    protected float currentHealth;

    [SerializeField] protected int phaseTransThreshold = 100;
    [SerializeField] protected int damage;
    [SerializeField] Animator bossAnim;
    [SerializeField] Transform beetleSpawn;
    [SerializeField] GameObject beetle;
    private bool phaseTransed = false;
     [SerializeField] private Transform groundCheck;
    private Transform playerTrans;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        currentTime = colorTime;
        currentHealth = MAXHealth;
        rb = GetComponent<Rigidbody2D>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
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
    }

    public int GetDamage()
    {
        return damage;
    }

    public bool GetDirection()
    {
        if(transform.position.x > playerTrans.position.x)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void flipBoss()
    {
        if (transform.position.x > playerTrans.position.x && isFacingRight || transform.position.x < playerTrans.position.x && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));

        }
    }

    public Transform GetPlayerPos()
    {
        return playerTrans;
    }

    public Transform GetLastPlayePos()
    {
        return lastPlayerPos;
    }

    public void SetLastPlayerPos()
    {
        lastPlayerPos = playerTrans;
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

    IEnumerator Die()
    {
        GameManager.Instance.playSFX(bossDeath.name, true);
        yield return new WaitForSeconds(0.2f);
        Destroy(this);
    }

}
