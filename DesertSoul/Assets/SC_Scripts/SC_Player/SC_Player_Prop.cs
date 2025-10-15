using System.Collections;
using System.Runtime.InteropServices.ComTypes;
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

    [SerializeField] private float sunStackDamageFrequency = 1f;
    private float sunStackDamageTimer;

    private int sunDamage;

    private SpriteRenderer sprite;
    private Color originalColor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sunStackDamageTimer = sunStackDamageFrequency;
        sunStackTimer = sunStackRemovalFrequency;
        GameManager.Instance.newScene();
       
        sprite = gameObject.GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inSunlight && currentSunStacks > 0)
        {
            depleteSunStacks();
        }
        if(currentSunStacks >= sunStacksBeforeDamage)
        {
            SunDamageTimer();
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

    public void giveSunStack(int damage)
    {
        inSunlight = true;
        sunDamage = damage;
        if (currentSunStacks < maxSunStacks)
        {
            currentSunStacks++;
        }
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

    private void SunDamageTimer()
    {
        if (sunStackDamageTimer > 0)
        {
            sunStackDamageTimer -= Time.deltaTime;
            if (sunStackDamageTimer <= 0)
            {
                StartCoroutine(FlashRed(0.2f));
                sunStackDamageTimer = sunStackDamageFrequency;
            }
        }
    }

    IEnumerator FlashRed(float duration)
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(duration);
        sprite.color = originalColor;
    }
}
