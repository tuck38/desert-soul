using UnityEngine;
using System.Collections.Generic;

public class SC_HurtBox : MonoBehaviour
{
    //Sythe hurtbox, self explanitory 

    public SC_Attack_Base currentAttack;

    //The enemy that is currently being affected by some attack EX:the drills carry
    private List<SC_Enemy_Base> currentEnemies;

    [SerializeField] private Transform carryPoint;
    [SerializeField] private ParticleSystem PlayerHitParticles;

    private void Start()
    {
        currentEnemies = new List<SC_Enemy_Base>();
    }

    void Update()
    {
        
    }

    public void StopEnemyLock()
    {
        foreach(SC_Enemy_Base enemy in currentEnemies)
        {
            enemy.setLocked(false);
        }
        currentEnemies = new List<SC_Enemy_Base>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            PlayerHitParticles.Play();
            SC_Enemy_Base enemy = collision.gameObject.GetComponent<SC_Enemy_Base>();
            if(enemy != null && currentAttack != null)
            {
                if(currentAttack.getAttackType() == AttackType.drillSide & !currentEnemies.Contains(enemy))
                {
                    currentEnemies.Add(enemy);
                }
                enemy.TakeDamage(currentAttack, carryPoint);
                enemy.SetPlayer(gameObject.gameObject);
                enemy.Knockback(currentAttack.getAttackType());
            }
        }
        else if (collision.gameObject.tag == "ResourceNode")
        {
            SC_ResourceNode node = collision.gameObject.GetComponent<SC_ResourceNode>();
            bool canBreak = node.AttackTypeToBreakNode() == currentAttack.getAttackType() || node.AttackTypeToBreakNode() == AttackType.all;
            if (node != null && currentAttack != null && canBreak)
            {
                node.TakeDamage(currentAttack.getDamage());
            }
        }
        else if (collision.gameObject.tag == "DrillableTerrain")
        {
            Debug.Log("terrain");
            SC_Drillable_Terrain terrain = collision.gameObject.GetComponent<SC_Drillable_Terrain>();
            if (terrain != null && currentAttack != null)
            {
                if(currentAttack.getAttackType() == AttackType.drillDown || currentAttack.getAttackType() == AttackType.drillSide)
                {
                    Debug.Log("terrain drill");
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
            SC_Dungball dungball = collision.gameObject.GetComponent<SC_Dungball>();
            if (dungball != null && (currentAttack.getAttackType() == AttackType.drillSide || currentAttack.getAttackType() == AttackType.drillDown))
            {
                dungball.DamageBall();
            }
        }
    }
}
