using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class respawnmanager : MonoBehaviour
{
    [SerializeField] SpriteRenderer rune1;

    [SerializeField] Light2D light1;

    [SerializeField] Light2D light2;

    [SerializeField] Light2D lightSpawn;

    [SerializeField] SpriteRenderer rune2;

    [SerializeField] Animator Animation;

    [SerializeField] float spawnTime;

    [SerializeField] UnityEngine.UI.Image Screen;

    [SerializeField] float fadeFromBlack;

    [SerializeField] float fadeToWhite;

    [SerializeField] float animTimer;

    [SerializeField] float setUpTime;

    private float currentSetupTime;

    private float currentAnimTime = 0;

    private float currentFadeBlackTimer = 0;

    private float currentFadeWhiteTimer = 0;

    private float timeToSpawn = 0;

    [SerializeField] float timeToGlow;

    private bool respawn = true;
    private float currentTimer = 0;

    private float currentGlow;

    [SerializeField] SC_Player_Move playerMove;

    [SerializeField] SpriteRenderer playerSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(respawn == true)
        {

        if(setUpTime >= currentSetupTime)
        {
                currentSetupTime += Time.deltaTime;
        }    
        else if(fadeFromBlack >= currentFadeBlackTimer)
        {
            currentFadeBlackTimer += Time.deltaTime;
            float percentegeComplete = currentFadeBlackTimer/ fadeFromBlack;

            float currentFade = Mathf.Lerp(1, 0, percentegeComplete);

            Screen.color = new UnityEngine.Color(Screen.color.r, Screen.color.g, Screen.color.b, currentFade);
        }
        else if(timeToGlow >= currentTimer)
        {
            currentTimer += Time.deltaTime;
            float percentegeComplete = currentTimer/timeToGlow;

            currentGlow = Mathf.Lerp(0, 1, percentegeComplete);

            rune1.color = new UnityEngine.Color(rune1.color.r, rune1.color.g, rune1.color.r, currentGlow);
            light1.intensity = currentGlow;
            rune2.color = new UnityEngine.Color(rune1.color.r, rune1.color.g, rune1.color.r, currentGlow);
            light2.intensity = currentGlow;
        }
        else if(animTimer >= currentAnimTime)
        {
            currentAnimTime += Time.deltaTime;
            lightSpawn.intensity = 1;
            Animation.SetBool("Respawn", true); 
        }
        else if(fadeToWhite >= currentFadeWhiteTimer)
        {
            Screen.color = new UnityEngine.Color(UnityEngine.Color.white.r, UnityEngine.Color.white.g, UnityEngine.Color.white.b, Screen.color.a);

            currentFadeWhiteTimer += Time.deltaTime;

            float percentegeComplete = currentFadeWhiteTimer / fadeToWhite;

            float currentFade = Mathf.Lerp(0, 1, percentegeComplete);

            Screen.color = new UnityEngine.Color(Screen.color.r, Screen.color.g, Screen.color.b, currentFade);
            if(currentFade >= 0.7)
            {
                respawned();
                currentFadeWhiteTimer = 200;
                respawn = false;
                Screen.color = new UnityEngine.Color(Screen.color.r, Screen.color.g, Screen.color.b, 0);
            }
        }
        }

    }

    void Glow()
    {
        
    }

    public void respawned()
    {
        lightSpawn.intensity = 0;

        GameObject.Destroy(Animation.gameObject);

        playerMove.isFirstRoom(false);

        playerMove.SetCanMove(true);

        playerSprite.color = new UnityEngine.Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1);

    }
}
