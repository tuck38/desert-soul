using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//This script is used to keep track of health, possible materials, and other recources that may be introduced
public class SC_Player_Prop : MonoBehaviour
{
    public static Action<ResouceTypes, int> OnResourcesAmountChanged;

    [SerializeField] private SC_Player_Move move;

    [SerializeField] public int maxHealth;
    [Tooltip("Exists to visualize current health in inspector, modifying in inspector won't change player's current health")]
    [SerializeField] private int currentHealthProxy;
    public static int currentHealth { get; private set; } = int.MinValue;

    [SerializeField] private SC_Player_HUD HUD;

    [SerializeField] private Text stoneText;
    [SerializeField] private Text FruitText;

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

    public static int stoneMaterialCount { get; private set; } = 0;
    public static int twineMaterialCount { get; private set; } = 0;
    public static int fruitMaterialCount { get; private set; } = 0;
    public static int iceMaterialCount { get; private set; } = 0;

    //[Header("Wwise Events")]
    //public AK.Wwise.Event playerDamaged;
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
        //Debug.Log($"Health {currentHealth}, Materials {stoneMaterialCount} {twineMaterialCount} {fruitMaterialCount} {iceMaterialCount}");
        if (currentHealth == int.MinValue)
        {
            currentHealth = maxHealth;
            currentHealthProxy = currentHealth;
        }
        sunStackDamageTimer = sunStackDamageFrequency;
        sunStackTimer = sunStackRemovalFrequency;
        GameManager.Instance.newScene();
       
        sprite = gameObject.GetComponent<SpriteRenderer>();
        originalColor = sprite.color;

        HUD.UpdateHealthUI(currentHealth, maxHealth);
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
        if (Input.GetKeyDown("p"))
        {
            currentHealth -= 1;
        }
    }
    
    public void TakeDamage(int dmg)
    {
        if (currentHealth > dmg)
        {
            Debug.Log("sdfsdf");
            currentHealth = currentHealth - dmg;
            currentHealthProxy = currentHealth;
            HUD.UpdateHealthUI(currentHealth, maxHealth);
            move.SetIFrames();
            //playerDamaged.Post(gameObject);
        }
        else
        {
            //TODO: Good for now but change this to restarting from save
            currentHealth = int.MinValue;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
    }

    public bool can_heal()
    {
        if (currentHealth < maxHealth)
        {
            print(currentHealth);
            return true;
        }
        return false;
    }
    

    public void IncreaseHP()
    {
        currentHealth += 1;
        HUD.UpdateHealthUI(currentHealth, maxHealth);
        print(currentHealth);
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
                stoneText.text = stoneMaterialCount.ToString();
                break;
            case ResouceTypes.TWINE:
                twineMaterialCount += amountOfResouceChanged;
                break;
            case ResouceTypes.FRUIT:
                fruitMaterialCount += amountOfResouceChanged;
                FruitText.text = stoneMaterialCount.ToString();
                break;
            case ResouceTypes.ICE:
                iceMaterialCount += amountOfResouceChanged;
                break;
        }
    }
}
