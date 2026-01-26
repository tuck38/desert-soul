using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;
using Unity.VisualScripting;

public class SC_Sythe : MonoBehaviour
{
    /*Script originally created to allow player to engage with a combat system, changed to allow for simple 1 off 
    attacks until combat sysem is more nailed down*/
    
    //Script where the attacks are executed, and communication with the animator occures

    [SerializeField] private Animator animator;

    //List of attacks for current weapon
    [SerializeField] protected List<SC_Attack_Base> Attacks;
    //List that holds the chain of moves that hav been executed
    [SerializeField] protected List<AttackType> combo;
    //depricated 
    //[SerializeField] private float ComboTimer;

    //projectile spawn in front of player, used if attack uses spawned objects (such as bullets)
    [SerializeField] private GameObject projectileSpawn;

    [SerializeField] private SC_HurtBox HurtBox;

    //each attack has a timer assosiated with them that is set as soon as the attack is executed
    private float currentAttackLength;
    //Timer that counts up until the attack animation should be concluded
    private float attackTimer;

    private bool isAttacking;

    [Header("Wwise Events")]
    public AK.Wwise.Event sytheAttack;
    private void OnEnable()
    {
        SC_UpgradeNode.OnNodeUnlocked += ApplyUpgrade;
    }

    private void OnDisable()
    {
        SC_UpgradeNode.OnNodeUnlocked -= ApplyUpgrade;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAttacking = false;
         HurtBox.enabled = false;
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
        //bool match = true;

        //iterates through the weapons list of attacks on the weapon, and tests their
        //requirments against the current combo list to find the correct attack to be
        //executed based on the next input
        /*for(int i = 0; i < Attacks.Count; i++)
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
        }*/

        animator.SetInteger("Attack", Attacks[0].getID());
        HurtBox.currentAttack = Attacks[0];
        isAttacking = true;
        attackTimer = 0f;
        currentAttackLength = Attacks[0].getTime();
        animator.SetBool("isAttacking", isAttacking);
        Attacks[0].doAttack(projectileSpawn.transform);
        HurtBox.enabled = true;
        // Sythe sound attack
        sytheAttack.Post(gameObject);

        //if no moves are found that equal the current combo, clear the combo and run the function again to preform a basic move
        //EndCombo();
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
            HurtBox.enabled = false;
            EndCombo();
            animator.SetBool("isAttacking", isAttacking);
        }

    }

    public List<AttackType> GetCombo()
    {
        return combo;
    }

    /// <summary>
    /// Apply upgrade based on unlocked node / passed in upgrade type
    /// </summary>
    /// <param name="weaponType"></param>
    /// <param name="weaponUpgrade"></param>
    private void ApplyUpgrade(WeaponTypes weaponType, WeaponUpgrades weaponUpgrade)
    {
        if (weaponType != WeaponTypes.SCYTHE) return;

        switch(weaponUpgrade)
        {
            case WeaponUpgrades.IncreaseDamage:
                // Scythe damage + 0.5 (default is 1)
                break;
            case WeaponUpgrades.IncreaseMaterialsGathered:
                // Material nodes gathered with Scythe + 10%
                break;
            case WeaponUpgrades.IncreaseBossDamage:
                // Scythe damage vs Boss + 0.5
                break;
            case WeaponUpgrades.IncreaseNumberOfAttacks:
                // Adds one additional slice to the player�s primary attack. The player can press the attack button one additional time after the initial press to perform this second slice. This only applies to the player�s neutral, left, and right attacks. Hitting an enemy with any hits greater than one autolocks the player and the enemy together, halting momentum. This upgrade stacks.
                break;
            case WeaponUpgrades.ChanceAtDoubleReward:
                // Enemies slain using the scythe have a chance at dropping x2 money (10% chance) Does not apply to bosses.
                break;
            case WeaponUpgrades.ChanceAtHealingFromAttack:
                // Dealing damage to enemies using the scythe has a chance of healing the player. (3% chance to recover 1 HP on-hit)
                break;
        }
    }
}
