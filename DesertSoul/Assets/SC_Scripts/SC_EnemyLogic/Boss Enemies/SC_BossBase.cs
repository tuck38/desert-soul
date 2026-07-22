using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SC_BossBase : SC_Enemy_Base
{
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

    public float halfWidth;

    public float halfHeight;

    private bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        slider = healthBar.GetComponent<Slider>();
        baseColor = sprite.color;
        halfHeight = sprite.bounds.extents.y;
        halfWidth = sprite.bounds.extents.x;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
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

        //takes damage and returns true if the attack killed the enemy
    public override bool TakeDamage(SC_Attack_Base attack, Transform carryPoint)
    {
        currentHealth -= attack.getDamage();
        slider.value = currentHealth / MAXHealth;
        if (currentHealth <= 100 && phaseTransed == false)
        {
            GameObject obj = Instantiate(beetle, beetleSpawn.position, beetleSpawn.rotation);
            phaseTransed = true;
        }
        if (currentHealth <= 0)
        {
            //death "cutscene"
            SceneManager.LoadScene("DemoOverScene");
            Destroy(gameObject);
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
        healthBar.SetActive(enable);
        slider.value = 1;
    }

    public void BossMusic()
    {
        GameManager.Instance.playSong(bossTheme.name);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public Transform getBossTransform()
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

}
