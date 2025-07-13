using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;
using Unity.VisualScripting;

public class WeaponBase : MonoBehaviour
{

    [SerializeField] private SpriteRenderer WeaponSprite;
    [SerializeField] protected List<AttackBase> Attacks;
    [SerializeField] protected List<AttackType> combo;
    [SerializeField] private float ComboTimer;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject projectileSpawn;

    [SerializeField] private WP_HitBox HitBox;

    private float currentAttackTimer;
    private float attackTime;
    private bool isAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttacking == true)
        {
            AttackTimer();
        }
    }

    public void AddAttack(AttackType attack)
    {
        bool match = true;

        for(int i = 0; i < Attacks.Count; i++)
        {
            if (Attacks[i].getAttackType() == attack)
            {
                if (combo.Count == Attacks[i].getRequire().Count)
                {
                    for (int j = 0; j < Attacks[i].getRequire().Count; j++)
                    {
                        if (combo[j] != Attacks[i].getRequire()[j])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        animator.SetInteger("Attack", Attacks[i].getID());
                        HitBox.currentAttack = Attacks[i];
                        isAttacking = true;
                        attackTime = 0f;
                        currentAttackTimer = Attacks[i].getTime();
                        animator.SetBool("isAttacking", isAttacking);
                        Attacks[i].doAttack(projectileSpawn.transform);
                        combo.Add(attack);

                        return;
                    }
                }
            }
        }
        //if no moves are found that equal the current combo, clear the combo and run the function again to preform a basic move
        EndCombo();
        //AddAttack(attack, anim);
        return;
    }

    public void EndCombo()
    {
        combo.Clear();
    }

    private void AttackTimer()
    {
        if (attackTime < currentAttackTimer)
        {
            attackTime += Time.deltaTime;
        }
        else
        {
            isAttacking = false;
            EndCombo();
            animator.SetBool("isAttacking", isAttacking);
        }

    }

    public List<AttackType> GetCombo()
    {
        return combo;
    }
}
