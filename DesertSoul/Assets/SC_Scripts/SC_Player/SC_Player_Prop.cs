using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//This script is used to keep track of health, possible materials, and other recources that may be introduced
public class SC_Player_Prop : MonoBehaviour
{
    public static Action<ResouceTypes, int> OnResourcesAmountChanged;

    [SerializeField] private SC_Player_Move move;
    [SerializeField] private ParticleSystem PlayerDamagedParticles;
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

    //Resource Bullshit

    [SerializeField] TextMeshProUGUI textRC;
    [SerializeField] Image imageRC;

    [SerializeField] Image bgRC;

    [SerializeField] Sprite twine;

    [SerializeField] Sprite rock;

    [SerializeField] Sprite money;

    [SerializeField] float fadeTimer;

    float currentFade;

    [SerializeField] float appearTime;

    float currentAppearTime;

    [SerializeField] float fadeOutTime;

    [SerializeField] float currentFadeOutTime;

    bool resourceUIActive = false;

    int stoneUI = 0;

    int twineUI = 0;

    [SerializeField] TextMeshProUGUI plus;

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

        HUD.UpdateHealthUI(currentHealth, maxHealth, false);
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

        //this is a suprise tool that will help us later
        /*if(currentAppearTime < appearTime)
        {
            currentAppearTime += Time.deltaTime;
        }
        else if(currentAppearTime >= appearTime && resourceUIActive == true)
        {
            currentFade = 0;
        }*/

        if(currentFade < fadeTimer)
        {
            currentFade += Time.deltaTime;
        }
        else if(resourceUIActive == true)
        {
            textRC.color = new Color(textRC.color.r, textRC.color.g, textRC.color.b, 0);
            imageRC.color = new Color(imageRC.color.r, textRC.color.g, textRC.color.b, 0);
            bgRC.color = new Color(bgRC.color.r, bgRC.color.g, bgRC.color.b, 0);
            plus.color = new Color(plus.color.r, plus.color.g, plus.color.b, 0);
            stoneUI = 0;
            twineUI = 0;
            resourceUIActive = false;
        }


    }
    
    public void TakeDamage(int dmg)
    {
        
        if (currentHealth > dmg)
        {
            PlayerDamagedParticles.Play();
            currentHealth = currentHealth - dmg;
            currentHealthProxy = currentHealth;
            HUD.UpdateHealthUI(currentHealth, maxHealth, false);
            move.SetIFrames();
            
            //playerDamaged.Post(gameObject);
        }
        else
        {
            //TODO: Good for now but change this to restarting from save
            currentHealth = int.MinValue;
            GameManager.Instance.playerDead();
            
        }
    }

    public bool can_heal()
    {
        if (currentHealth < maxHealth)
        {
            return true;
        }
        return false;
    }
    

    public void IncreaseHP()
    {
        currentHealth += 1;
        HUD.UpdateHealthUI(currentHealth, maxHealth, true, 1);
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
                TakeDamage(sunDamage);
                sunStackDamageTimer = sunStackDamageFrequency;
            }
        }
    }

    public void UpdateText()
    {
        stoneText.text = stoneMaterialCount.ToString();
        FruitText.text = twineMaterialCount.ToString();
    }

    private void ResourceUI()
    {
        
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
    public void UpdateResources(ResouceTypes resourceType, int amountOfResouceChanged)
    {
        switch (resourceType)
        {
            case ResouceTypes.STONE:
                stoneMaterialCount += amountOfResouceChanged;
                stoneUI += amountOfResouceChanged;
                stoneText.text = stoneMaterialCount.ToString();
                if(resourceUIActive == false)
                {
                    currentFade = 0;
                    resourceUIActive = true;
                    imageRC.sprite = rock;
                    textRC.color = new Color(textRC.color.r, textRC.color.g, textRC.color.b, 1);
                    imageRC.color = Color.white;
                    bgRC.color = new Color(bgRC.color.r, bgRC.color.g, bgRC.color.b, 1);
                    plus.color = new Color(plus.color.r, plus.color.g, plus.color.b, 1);
                    textRC.text = stoneUI.ToString();
                }
                else
                {
                    currentFade = 0;
                    textRC.text = stoneUI.ToString();
                }
                break;
            case ResouceTypes.TWINE:
                twineMaterialCount += amountOfResouceChanged;
                twineUI += amountOfResouceChanged;
                FruitText.text = twineMaterialCount.ToString();

                if(resourceUIActive == false)
                {
                    currentFade = 0;
                    resourceUIActive = true;
                    imageRC.sprite = rock;
                    textRC.text = twineUI.ToString();
                    textRC.color = new Color(textRC.color.r, textRC.color.g, textRC.color.b, 1);
                    imageRC.color = Color.white;
                    bgRC.color = new Color(bgRC.color.r, bgRC.color.g, bgRC.color.b, 1);
                    plus.color = new Color(plus.color.r, plus.color.g, plus.color.b, 1);
                }
                else
                {
                    currentFade = 0;
                    textRC.text = twineUI.ToString();
                }
                break;
            case ResouceTypes.FRUIT:
                fruitMaterialCount += amountOfResouceChanged;
                Debug.Log("fruit");
                break;
            case ResouceTypes.ICE:
                iceMaterialCount += amountOfResouceChanged;
                break;
        }
    }
}
