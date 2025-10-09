using UnityEngine;
using UnityEngine.SceneManagement;

//This script is used to keep track of health, possible materials, and other recources that may be introduced
public class SC_Player_Prop : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    //Sun Beam Variables, putting here for now
    [SerializeField] private int maxSunStacks;
    [SerializeField] private int sunStacksBeforeDamage;
    private int currentSunStacks = 0;
    private bool inSunlight = false;

    [SerializeField] private float sunStackRemovalFrequency = 0.2f;
    private float sunStackTimer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sunStackTimer = sunStackRemovalFrequency;
        GameManager.Instance.newScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inSunlight && currentSunStacks > 0)
        {
            depleteSunStacks();
        }
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

    public void giveSunStack()
    {
        inSunlight = true;
        currentSunStacks++;
    }

    public void OutOfSun()
    {
        inSunlight = false;
    }

    private void depleteSunStacks()
    {
        if (sunStackTimer > 0)
        {
            sunStackTimer -= Time.deltaTime;
            if (sunStackTimer <= 0)
            {
                currentSunStacks--;
                sunStackTimer = sunStackRemovalFrequency;
            }
        }
    }
}
