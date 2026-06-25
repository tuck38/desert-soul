using UnityEngine;
using UnityEngine.UI;

public class SC_BossBase : SC_Enemy_Base
{
    [SerializeField] Animator bossAnim;
    private Transform player;

    [SerializeField] GameObject healthBar;
    private Slider slider;

    [SerializeField] SpriteRenderer sprite;

    [SerializeField] Color baseColor;

    private bool isFacingRight = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        slider = healthBar.GetComponent<Slider>();
        baseColor = sprite.color;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
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

        //takes damage and returns true if the attack killed the enemy
    public override bool TakeDamage(SC_Attack_Base attack, Transform carryPoint)
    {
        currentHealth -= attack.getDamage();
        slider.value = currentHealth / MAXHealth;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    public void StartFight()
    {
        bossAnim.SetTrigger("FightStart");
    }

    public void enableHPBar(bool enable)
    {
        healthBar.SetActive(enable);
        slider.value = 1;
    }

    public void BossRoar()
    {
        
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
