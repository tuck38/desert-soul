using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum WeaponTypes
{
    SCYTHE,
    DRILL,
    HOOKSHOT
}

public enum NodeLevel
{
    I,
    II,
    III,
    IV,
    V,
    VI
}

public enum WeaponUpgrades
{
    IncreaseDamage,
    IncreaseMaterialsGathered,
    IncreaseBossDamage,
    IncreaseNumberOfAttacks,
    ChanceAtDoubleReward,
    ChanceAtHealingFromAttack,

    IncreaseAttackDistance,
    IncreaseAttackTravelSpeed,
    BounceOffTerrain,
    DrillGroundAttackIncreaseDamageZones,
    LaunchEnemies
}

public class SC_UpgradeNode : MonoBehaviour
{
    public static Action<WeaponTypes, WeaponUpgrades> OnNodeUnlocked;

    [SerializeField] WeaponTypes nodeWeaponType;
    [SerializeField] NodeLevel nodeLevel;
    [SerializeField] WeaponUpgrades nodeBuff;
    [SerializeField] bool nodeUnlockable = true;
    [SerializeField] List<SC_UpgradeNode> nodesToUnlockAfterCurrentUnlocking;

    bool nodePurchased = false;

    private void Start()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = nodeLevel.ToString();
        if(!nodeUnlockable) GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// When Node is Pressed, fire off event telling player what buff to unlock and disable the node from being pressed again
    /// </summary>
    public void NodePressed()
    {
        OnNodeUnlocked?.Invoke(nodeWeaponType, nodeBuff);
        nodeUnlockable = false;
        nodePurchased = true;
        GetComponent<Button>().interactable = false;
        foreach(var nodes in nodesToUnlockAfterCurrentUnlocking)
        {
            nodes.UnlockNode();
        }
    }

    protected void UnlockNode()
    {
        if (!nodePurchased)
        {
            nodeUnlockable = true;
            GetComponent<Button>().interactable = true;
        }
    }
}
