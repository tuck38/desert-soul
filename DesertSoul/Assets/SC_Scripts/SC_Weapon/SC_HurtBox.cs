using UnityEngine;
using System.Collections.Generic;

public class SC_HurtBox : MonoBehaviour
{
    //Sythe hurtbox, self explanitory 

    public SC_Attack_Base currentAttack;

    //The enemy that is currently being affected by some attack EX:the drills carry
    private List<SC_Enemy_Base> currentEnemies;

    [SerializeField] GameObject venture;

    [SerializeField] GameObject venture2;

    [SerializeField] private Transform carryPoint;
    [SerializeField] private ParticleSystem PlayerHitParticles;

    [SerializeField] SC_Sythe sythe;

    private void Start()
    {
        currentEnemies = new List<SC_Enemy_Base>();
    }

    void Update()
    {
        
    }

    public void StopEnemyLock()
    {
        /*foreach(SC_Enemy_Base enemy in currentEnemies)
        {
            enemy.setLocked(false);
        }
        currentEnemies = new List<SC_Enemy_Base>();*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            PlayerHitParticles.Play();
            SC_RattleBase enemy = collision.gameObject.GetComponent<SC_RattleBase>();


            if(enemy != null && currentAttack != null)
            {
                if(!enemy.IsBoss())
                {
                    enemy.TakeDamage(currentAttack, sythe.GetDamage(), carryPoint, venture, venture2);
                    enemy.SetPlayer(gameObject.gameObject);
                    enemy.Knockback(currentAttack.getAttackType(), currentAttack.GetkbMult());
                }
                //PlayerHitParticles.Play();
                //Build getting here, not calling takedamage?
            }
            else
            {
                SC_DungBeetle beetle = collision.gameObject.GetComponent<SC_DungBeetle>();

                if(beetle != null && currentAttack != null)
                {
                    beetle.TakeDamage(currentAttack, carryPoint);

                }
            }
        }
        else if(collision.gameObject.tag == "Boss")
        {
            SC_BossBase boss = collision.gameObject.GetComponent<SC_BossBase>();
            boss.TakeBossDamage(currentAttack, sythe.GetDamage());
        }
        else if (collision.gameObject.tag == "ResourceNode")
        {
            SC_ResourceNode node = collision.gameObject.GetComponent<SC_ResourceNode>();
            bool canBreak = false;
            if(currentAttack != null)
            {
                canBreak = node.AttackTypeToBreakNode() == currentAttack.getAttackType() || node.AttackTypeToBreakNode() == AttackType.all;
            }
            if (node != null && currentAttack != null && canBreak)
            {
                node.TakeDamage(currentAttack.getDamage());
            }
        }
        else if (collision.gameObject.tag == "DrillableTerrain")
        {
            SC_Drillable_Terrain terrain = collision.gameObject.GetComponent<SC_Drillable_Terrain>();
            if (terrain != null && currentAttack != null)
            {
                if(currentAttack.getAttackType() == AttackType.drillDown || currentAttack.getAttackType() == AttackType.drillSide)
                {
                    terrain.BreakTerrain();
                }
            }
        }
        else if(collision.gameObject.tag == "Fruit")
        {
            collision.gameObject.GetComponent<SC_Fruit>().FruitHit();
        }
        else if (collision.gameObject.tag == "Dungball")
        {
            SC_DungBall dungball = collision.gameObject.GetComponent<SC_DungBall>();
            if (dungball != null && (currentAttack.getAttackType() == AttackType.drillSide || currentAttack.getAttackType() == AttackType.drillDown))
            {
                dungball.Break();
            }
        }
    }
}
