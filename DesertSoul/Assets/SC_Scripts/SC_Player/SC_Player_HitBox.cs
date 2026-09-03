using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class SC_Player_HitBox : MonoBehaviour
{

    [SerializeField] private SC_Player_Prop prop;
    [SerializeField] private SC_Player_Move player;
    [SerializeField] private SC_Sythe scythe;

    [SerializeField] private Vector2 fixedLaunchVector;


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
                if(scythe.getParry())
                {
                    
                }
                else if(scythe.getBlocking())
                {
                    prop.TakeDamage(enemy.GetDamage() / 2);
                    //enemy KB and player KB
                }
                else
                {
                    //when enemies "do" attacks instead of a fixed contact value, this will be changed to be based on their code
                    prop.TakeDamage(enemy.GetDamage());
                    player.Knockback(collision.gameObject, fixedLaunchVector);
                    player.SetIFrames();
                }
            }
            SC_RattleBase rattle = collision.gameObject.GetComponent<SC_RattleBase>();
            if(rattle != null && !player.iFramesActive)
            {
                if(scythe.getParry())
                {
                    rattle.SetPlayer(player.gameObject);
                    rattle.Knockback(AttackType.primary, scythe.parryKB);
                    prop.Parry(scythe.blockInitialStamina);
                    prop.IncreaseFlow(scythe.parryFlow);
                }
                else if(scythe.getBlocking())
                {
                    rattle.SetPlayer(player.gameObject);
                    prop.TakeDamage(rattle.GetDamage() / 2);
                    rattle.Knockback(AttackType.primary, scythe.blockKB);
                    player.Knockback(collision.gameObject, new Vector2(fixedLaunchVector.x / 2, 0));
                    prop.IncreaseFlow(scythe.blockFlow);
                }
                else
                {
                    prop.TakeDamage(rattle.GetDamage());
                    player.Knockback(collision.gameObject, fixedLaunchVector);
                    player.SetIFrames();
                }
            }
        }
        else if (collision.gameObject.tag == "Dungball")
        {
            SC_DungBall enemy = collision.gameObject.GetComponent<SC_DungBall>();
            if (enemy != null && !player.iFramesActive)
            {
                if(scythe.getParry())
                {
                    prop.Parry(scythe.blockInitialStamina);
                    Destroy(enemy.gameObject);
                    prop.IncreaseFlow(scythe.parryFlow);
                }
                else if(scythe.getBlocking())
                {
                    prop.TakeDamage(enemy.GetDamage() / 2);
                    player.Knockback(collision.gameObject, new Vector2(fixedLaunchVector.x / 2, 0));
                    prop.IncreaseFlow(scythe.blockFlow);
                    Destroy(enemy.gameObject);
                }
                else
                {
                    prop.TakeDamage(enemy.GetDamage());
                    player.Knockback(collision.transform.gameObject, fixedLaunchVector);
                    player.SetIFrames();
                    Destroy(enemy.gameObject);
                }
            }
        }
        else if(collision.gameObject.tag == "Boss")
        {
            SC_BossBase boss = collision.gameObject.GetComponent<SC_BossBase>();
            if(boss != null && !player.iFramesActive)
            {
                if(scythe.getParry())
                {
                    if(boss.Parried(AttackType.primary, scythe.parryKB))
                    {
                        prop.Parry(scythe.blockInitialStamina);
                        prop.IncreaseFlow(scythe.parryFlow);
                    }
                }
                else if(scythe.getBlocking())
                {
                    prop.TakeDamage(boss.GetDamage() / 2);
                    player.Knockback(collision.gameObject, new Vector2(fixedLaunchVector.x / 2, 0));
                    prop.IncreaseFlow(scythe.blockFlow);

                }
                else
                {
                    prop.TakeDamage(boss.GetDamage());
                    player.Knockback(collision.transform.gameObject, fixedLaunchVector);
                    player.SetIFrames();
                }
            }
        }
    }
}
