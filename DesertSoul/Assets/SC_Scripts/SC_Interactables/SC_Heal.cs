using UnityEngine;

public class SC_Heal : SC_Interactable
{

    protected override void DoAction()
    {
        //if interacted with for a set amount of time it despawns
        if (playerProp.can_heal())
        {
            playerProp.IncreaseHP();
            this.gameObject.SetActive(false);
        }
    }

    protected override void Update()
    {
        base.Update();

    }
}
