using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SC_Player_HUD : MonoBehaviour
{
    [SerializeField] private List<GameObject> Health;

    //Health UI Creation
    [SerializeField] private GameObject healthNode;

    [SerializeField] private GameObject healthParent;

    [SerializeField] private Slider stamSlider;

    [SerializeField] private Slider flowSlider;

    [SerializeField] private float healthSpacing;
    public int mapPart;

    private int createdHealth = 1;

    private int activeHealth = 1;

    //Flask UI Creation

    [SerializeField] private List<GameObject> flasks;

    [SerializeField] private GameObject flask;

    [SerializeField] private GameObject flaskParent;

    [SerializeField] private float flaskSpacing;

    private int createdFlasks = 1;

    private int activeFlasks = 1;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth, bool healing, int healAmount = 0)
    {
        int healthOfCreation = 0;
        //creating the health nodes at the begining
        if(createdHealth < maxHealth)
        {
            for(int i = 0; i < maxHealth - createdHealth; i++)
            {
                Transform trans = Health[Health.Count - 1].transform;
                Vector3 pos = new Vector3(trans.position.x + healthSpacing, trans.position.y, trans.position.z);
                GameObject node = Instantiate(healthNode, pos, Quaternion.identity, healthParent.transform);
                healthOfCreation++;
                Health.Add(node);
            }
            createdHealth += healthOfCreation;
            activeHealth = createdHealth;
        }
        //taking damage
        else if (currentHealth < activeHealth)
        {
            int healthActive = activeHealth;
            for(int i = currentHealth; i < healthActive; i++)
            {
                Health[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                healthActive --;
            }
            activeHealth = healthActive;
        }
        //healing
        else if(healing)
        {
            for(int i = activeHealth; i < currentHealth; i++)
            {
                Health[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                activeHealth++;
            }
        }
    }

    public void UpdateFlaskUI(int currentFlasks, int maxFlasks, bool recharge, int flasksRegained = 0)
    {
        int flasksCreated = 0;

        //creating the flask nodes at the begining
        if(createdFlasks < maxFlasks)
        {
            for(int i = 0; i < maxFlasks - createdFlasks; i++)
            {
                Transform trans = flasks[flasks.Count - 1].transform;
                Vector3 pos = new Vector3(trans.position.x + flaskSpacing, trans.position.y, trans.position.z);
                GameObject node = Instantiate(flask, pos, Quaternion.identity, flaskParent.transform);
                flasksCreated++;
                flasks.Add(node);
            }
            createdFlasks += flasksCreated;
            activeFlasks = createdFlasks;
        }
        //consuming flask
        else if (currentFlasks < activeFlasks)
        {
            int flaskActive = activeFlasks;
            for(int i = currentFlasks; i < flaskActive; i++)
            {
                flasks[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                flaskActive --;
            }
            activeFlasks = flaskActive;
        }
        //restore flask
        else if(recharge)
        {
            for(int i = activeFlasks; i < currentFlasks; i++)
            {
                flasks[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                activeFlasks++;
            }
        }
    }
    

    public void UpdateStamUI(float current, float max)
    {
        float per = current / max;
        stamSlider.value = per;
    }

    public void UpdateFlowUI(float current, float max)
    {
        float per = current / max;
        flowSlider.value = per;
    }
}
