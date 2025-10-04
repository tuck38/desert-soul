using UnityEngine;
using System.Collections.Generic;

public class SC_HurtBox : MonoBehaviour
{
    //Sythe hurtbox, self explanitory 

    public SC_Attack_Base currentAttack;

    //The enemy that is currently being affected by some attack EX:the drills carry
    private List<SC_Enemy_Base> currentEnemies;

    [SerializeField] private Transform carryPoint;


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
            SC_Enemy_Base enemy = collision.gameObject.GetComponent<SC_Enemy_Base>();
            if(enemy != null && currentAttack != null)
            {
                if(currentAttack.getAttackType() == AttackType.drillSide & !currentEnemies.Contains(enemy))
                {
                    currentEnemies.Add(enemy);
                }
                enemy.TakeDamage(currentAttack, carryPoint);
            }
        }
        if (collision.gameObject.tag == "ResourceNode")
        {
            SC_ResourceNode node = collision.gameObject.GetComponent<SC_ResourceNode>();
            if (node != null && currentAttack != null)
            {
                node.TakeDamage(currentAttack.getDamage());
            }
        }
    }
}
