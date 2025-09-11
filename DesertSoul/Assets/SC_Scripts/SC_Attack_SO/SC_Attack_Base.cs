using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SC_Attack_Base : ScriptableObject
{
    //SO For different attacks, used for old combo system but can be easily adjusted so keeping it around

    [SerializeField] protected List<AttackType> requirments;
    [SerializeField] private int damage;
    [SerializeField] private float AtkTime;
    [SerializeField] private AttackType type;
    [SerializeField] private int attackID;
    [SerializeField] private GameObject projectile;

    public AttackType getAttackType()
    {
        return type;
    }

    public List<AttackType> getRequire()
    {
        return requirments;
    }

    public void doAttack(Transform transform)
    {
        if (type == AttackType.primary)
        {
            //Have this adjust animator instead because that handles hitboxes, and adjust code in SC_Sythe
            //dohitboxstuff
        }
        else if (type == AttackType.secondary)
        {
            //Spawn projectile
        }
    }

    public int getDamage()
    {
        return damage;
    }

    public int getID()
    { 
        return attackID; 
    }

    public float getTime()
    {
        return AtkTime;
    }
}
