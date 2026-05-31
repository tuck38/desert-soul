using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class SC_Drill : MonoBehaviour
{
    //Venture overwatch
    private Rigidbody2D rb;
    private SC_Player_Move playerMove;
    private Animator animator;


    [SerializeField] private int damage;
    [SerializeField] private float timeDrillin = .5f;
    [SerializeField] private float sideDrillSpeed = 5f;
    [SerializeField] private float downDrillSpeed = 5f;
    [SerializeField] private float drillCooldown = 1f;
    [SerializeField] private SC_HurtBox hurtbox;
    [SerializeField] private SC_Attack_Base attackSide;
    [SerializeField] private SC_Attack_Base attackDown;



    private float currentDrillCooldown = 0f;  
    private float currentTimeDrillin = 0f;

    private bool drillin = false;
    private bool drillinDown = false;

    private bool playerGrounded;



    //:3
    private Vector2 shmovement;

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
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerMove = gameObject.GetComponent<SC_Player_Move>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (drillin)
        {
            if (currentTimeDrillin <= timeDrillin)
            {
                currentTimeDrillin += Time.deltaTime;
                if (drillinDown)
                {
                    playerMove.Move(shmovement, downDrillSpeed, false);
                }
                else
                {
                    playerMove.Move(shmovement, sideDrillSpeed, false);
                }
            }
            else
            {
                shmovement = new Vector2(0, 0);
                //done drillin
                playerMove.Move(shmovement, 0, false);
                drillin = false;
                animator.SetBool("DrillSide", false);
                animator.SetBool("DrillDown", false);
                drillinDown = false;
                currentDrillCooldown = drillCooldown;
                playerMove.SetCanMove(true);
                hurtbox.StopEnemyLock();
            }
        }
        else
        {
            if (currentDrillCooldown > 0)
            {
                currentDrillCooldown -= Time.deltaTime;
            }
        }
    }

    public void Drill(bool isGrounded, bool isFacingRight)
    {
        //Drill Animation switch here 

        playerGrounded = isGrounded;
        if (isGrounded & currentDrillCooldown <= 0)
        {
            animator.SetBool("DrillSide", true);
            drillin = true;
            Debug.Log(drillin);
            attackSide.setTime(timeDrillin);
            hurtbox.currentAttack = attackSide;
            currentTimeDrillin = 0f;
            playerMove.SetCanMove(false);
            if (isFacingRight)
            {
                shmovement = new Vector2(1, 0);
            }
            else
            {
                shmovement = new Vector2(-1, 0);
            }
        }
        else if (!isGrounded && currentDrillCooldown <= 0)
        {
            animator.SetBool("DrillDown", true);
            drillin = true;
            drillinDown = true;
            attackDown.setTime(timeDrillin);
            hurtbox.currentAttack = attackDown;
            currentTimeDrillin = 0f;
            playerMove.SetCanMove(false);
            shmovement = new Vector2(0, -1);
        }
    }

    /// <summary>
    /// Apply upgrade based on unlocked node / passed in upgrade type
    /// </summary>
    /// <param name="weaponType"></param>
    /// <param name="weaponUpgrade"></param>
    private void ApplyUpgrade(WeaponTypes weaponType, WeaponUpgrades weaponUpgrade)
    {
        if (weaponType != WeaponTypes.DRILL) return;

        switch (weaponUpgrade)
        {
            case WeaponUpgrades.IncreaseDamage:
                // Drill damage + 0.5
                break;
            case WeaponUpgrades.IncreaseMaterialsGathered:
                // Material nodes gathered with Drill + 10%
                break;
            case WeaponUpgrades.IncreaseAttackDistance:
                // Max drill distance + 0.5 seconds
                break;
            case WeaponUpgrades.IncreaseAttackTravelSpeed:
                // Drill travel speed + 10%
                break;
            case WeaponUpgrades.BounceOffTerrain:
                // The player is able to use the airborne drill attack to bounce off of specific terrain.
                break;
            case WeaponUpgrades.DrillGroundAttackIncreaseDamageZones:
                // The drill�s ground attack now deals damage upon contact, and at the end of the attack
                break;
            case WeaponUpgrades.LaunchEnemies:
                // The player launches enemies in an upward arc upon contact with the drill ground attack (at the end of drill animation)
                break;
        }
    }
}