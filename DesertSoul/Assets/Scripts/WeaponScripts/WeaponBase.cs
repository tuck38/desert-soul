using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;
using Unity.VisualScripting;

public class WeaponBase : MonoBehaviour
{

    [SerializeField] private SpriteRenderer WeaponSprite;

    [SerializeField] private Animator animator;

    //List of attacks for current weapon
    [SerializeField] protected List<AttackBase> Attacks;
    //List that holds the chain of moves that hav been executed
    [SerializeField] protected List<AttackType> combo;
    //depricated 
    //[SerializeField] private float ComboTimer;

    //projectile spawn in front of player, used if attack uses spawned objects (such as bullets)
    [SerializeField] private GameObject projectileSpawn;

    [SerializeField] private WP_HurtBox HurtBox;

    //each attack has a timer assosiated with them that is set as soon as the attack is executed
    private float currentAttackLength;
    //Timer that counts up until the attack animation should be concluded
    private float attackTimer;

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

    //upon input by player, executes an attack based on the current combo and attack input
    public void AddAttack(AttackType attack)
    {
        bool match = true;

        //iterates through the weapons list of attacks on the weapon, and tests their
        //requirments against the current combo list to find the correct attack to be
        //executed based on the next input
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
                            //If no next attack is found, break out of the function
                            //Need to change this to execute a basic attack based on the input,
                            //instead of doing nothing
                            match = false;
                            break;
                        }
                    }
                    //If there is a match, sends an "ID" number to the animator to play the 
                    //matching attack and sets the attack timer
                    if (match)
                    {
                        animator.SetInteger("Attack", Attacks[i].getID());
                        HurtBox.currentAttack = Attacks[i];
                        isAttacking = true;
                        attackTimer = 0f;
                        currentAttackLength = Attacks[i].getTime();
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
        if (attackTimer < currentAttackLength)
        {
            attackTimer += Time.deltaTime;
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
