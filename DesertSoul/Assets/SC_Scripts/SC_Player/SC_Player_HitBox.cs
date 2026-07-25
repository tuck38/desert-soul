using UnityEngine;

public class SC_Player_HitBox : MonoBehaviour
{

    [SerializeField] private SC_Player_Prop prop;
    [SerializeField] private SC_Player_Move player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SC_Enemy_Base enemy = collision.gameObject.GetComponent<SC_Enemy_Base>();
            if (enemy != null && !player.iFramesActive)
            {
                prop.TakeDamage(enemy.GetDamage());
                player.Knockback(collision.gameObject);
                player.SetIFrames();
            }
            SC_RattleBase rattle = collision.gameObject.GetComponent<SC_RattleBase>();
            if(rattle != null && !player.iFramesActive)
            {
                prop.TakeDamage(rattle.GetDamage());
                player.Knockback(collision.gameObject);
                player.SetIFrames();
            }
        }
        else if (collision.gameObject.tag == "Dungball")
        {
            SC_DungBall enemy = collision.gameObject.GetComponent<SC_DungBall>();
            if (enemy != null && !player.iFramesActive)
            {
                prop.TakeDamage(enemy.GetDamage());
                player.Knockback(collision.transform.gameObject);
                player.SetIFrames();
                Destroy(enemy.gameObject);
            }
        }
        else if(collision.gameObject.tag == "Boss")
        {
            SC_BossBase boss = collision.gameObject.GetComponent<SC_BossBase>();
            if(boss != null && !player.iFramesActive)
            {
                prop.TakeDamage(boss.GetDamage());
                player.Knockback(collision.transform.gameObject);
                player.SetIFrames();
            }
        }
    }
}
