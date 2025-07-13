using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackBase : ScriptableObject
{
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
