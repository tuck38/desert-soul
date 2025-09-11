using UnityEngine;
using UnityEngine.SceneManagement;

//This script is used to keep track of health, possible materials, and other recources that may be introduced
public class SC_Player_Prop : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(int dmg)
    {
        if (currentHealth > dmg)
        {
            currentHealth = currentHealth - dmg;
            //TODO: update health UI
        }
        else
        {
            //TODO: Good for now but change this to restarting from save
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
    }
}
