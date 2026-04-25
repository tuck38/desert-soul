using UnityEngine;

public class SC_UpgradeCheck : MonoBehaviour
{
    public bool isDrill;

    

    // Update is called once per frame
    void Update()
    {
        if (isDrill){
            GetComponent<SC_Drill>().enabled = true;
            
        }
        else
        {
            GetComponent<SC_Drill>().enabled = false;
        }
    }


}
