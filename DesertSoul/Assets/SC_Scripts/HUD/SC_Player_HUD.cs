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

    [SerializeField] private GameObject momma;

    [SerializeField] private Slider stamSlider;

    [SerializeField] private Slider flowSlider;

    [SerializeField] private float spacing;
    public int mapPart;

    private int createdHealth = 1;

    private int activeHealth = 1;

    //Flask UI Creation

    [SerializeField] private List<GameObject> flasks;

    [SerializeField] private GameObject flask;

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
                Vector3 pos = new Vector3(trans.position.x + spacing, trans.position.y, trans.position.z);
                GameObject node = Instantiate(healthNode, pos, Quaternion.identity, momma.transform);
                healthOfCreation++;
                Health.Add(node);
            }
            createdHealth += healthOfCreation;
            activeHealth = createdHealth;
        }
        //taking damage
        else if (currentHealth < activeHealth)
        {
            for(int i = currentHealth; i < maxHealth; i++)
            {
                Health[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                activeHealth --;
            }
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
