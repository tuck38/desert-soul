using UnityEngine;

public class SC_Heal : SC_Interactable
{

    [SerializeField] int flasksToFill = 2;

    protected override void DoAction()
    {
        //if interacted with for a set amount of time it despawns
        if (playerProp.canRechargeFlask())
        {
            playerProp.RechargeFlasks(flasksToFill);
            this.gameObject.SetActive(false);
        }
    }

    protected override void Update()
    {
        base.Update();

    }
}
