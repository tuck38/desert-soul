using UnityEngine;

public class SC_Heal : SC_Interactable
{

    protected override void DoAction()
    {
        if (playerProp.can_heal())
        {
            playerProp.IncreaseHP();
        }
    }

    protected override void Update()
    {
        base.Update();

    }
}
