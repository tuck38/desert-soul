using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerProp : MonoBehaviour
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
