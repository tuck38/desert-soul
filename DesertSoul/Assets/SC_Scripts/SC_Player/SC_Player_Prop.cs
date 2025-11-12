using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script is used to keep track of health, possible materials, and other recources that may be introduced
public class SC_Player_Prop : MonoBehaviour
{
    public static Action<ResouceTypes, int> OnResourcesAmountChanged;

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

    [SerializeField] public int stoneMaterialCount { get; private set; }
    [SerializeField] public int twineMaterialCount { get; private set; }
    [SerializeField] public int fruitMaterialCount { get; private set; }
    [SerializeField] public int iceMaterialCount { get; private set; }

    bool materialTest = true;


    private void OnEnable()
    {
        OnResourcesAmountChanged += UpdateResources;
    }

    private void OnDisable()
    {
        OnResourcesAmountChanged -= UpdateResources;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sunStackDamageTimer = sunStackDamageFrequency;
        sunStackTimer = sunStackRemovalFrequency;
        GameManager.Instance.newScene();
       
        sprite = gameObject.GetComponent<SpriteRenderer>();
        originalColor = sprite.color;

        if (materialTest)
        {
            UpdateResources(ResouceTypes.STONE, 1000);
            UpdateResources(ResouceTypes.TWINE, 900);
            UpdateResources(ResouceTypes.FRUIT, 50);
            UpdateResources(ResouceTypes.ICE, 10);
        }
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

    /// <summary>
    /// Update the total amount of resources available to spend based on input
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="amountOfResouceChanged"></param>
    void UpdateResources(ResouceTypes resourceType, int amountOfResouceChanged)
    {
        switch (resourceType)
        {
            case ResouceTypes.STONE:
                stoneMaterialCount += amountOfResouceChanged;
                break;
            case ResouceTypes.TWINE:
                twineMaterialCount += amountOfResouceChanged;
                break;
            case ResouceTypes.FRUIT:
                fruitMaterialCount += amountOfResouceChanged;
                break;
            case ResouceTypes.ICE:
                iceMaterialCount += amountOfResouceChanged;
                break;
        }
    }
}
