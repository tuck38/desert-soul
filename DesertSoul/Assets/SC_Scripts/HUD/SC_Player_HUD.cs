using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SC_Player_HUD : MonoBehaviour
{
    [SerializeField] private List<GameObject> Health;

    [SerializeField] private GameObject healthNode;

    [SerializeField] private GameObject momma;
    [SerializeField] private float spacing;

    private int createdHealth = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        int healthOfCreation = 0;
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
        }
        else 
        {
        for(int i = 0; i < maxHealth - currentHealth; i++)
        {
            Health[Health.Count - i - 1].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        
        }
        createdHealth += healthOfCreation;
    }
}
