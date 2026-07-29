using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SC_Menus_Start : MonoBehaviour
{

    [SerializeField] string LastScene;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] RectTransform creditTransform;
    [SerializeField] GameObject mainScreen;

    [SerializeField] GameObject main;
    public float credSpd;
    //[Header("Wwise Events")]
    //public AK.Wwise.Event mainMenuButtons;
    bool activeCredits = false;

    bool fadeOut = false;
    [SerializeField] float fadeFromBlack;

    [SerializeField] UnityEngine.UI.Image screen;

    float currentFadeBlackTimer = 0;

    [SerializeField] AudioClip startSound;


    void Start()
    {
        EventSystem.current.SetSelectedGameObject(main);
    }
    void Update()
    {
        if (activeCredits)
        {
            creditTransform.anchoredPosition += new Vector2(0, +credSpd * Time.deltaTime);
        }
        else if (!activeCredits)
        {
            creditTransform.anchoredPosition = new Vector2(0,0);
        }

        if(fadeOut)
        {
            if(fadeFromBlack >= currentFadeBlackTimer)
            {
                currentFadeBlackTimer += Time.deltaTime;
                float percentegeComplete = currentFadeBlackTimer/ fadeFromBlack;

                float currentFade = Mathf.Lerp(0, 1, percentegeComplete);

                screen.color = new UnityEngine.Color(screen.color.r, screen.color.g, screen.color.b, currentFade);
            }
            else
            {
                fadeOut = false; 
                SceneManager.LoadScene("Room01_AreaTown");
            }
        }
    }

    public void StartGame ()
    {
       // mainMenuButtons.Post(gameObject);
        currentFadeBlackTimer = 0;
        GameManager.Instance.playSFX(startSound.name);
        fadeOut = true;
    }

    public void Options()
    {
        //mainMenuButtons.Post(gameObject);
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
        StartCoroutine(rollCreds());
        
    }

    public void Back()
    {
        creditsPanel.SetActive(false);
        activeCredits = false;
        //optinsMenu.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void QuitGame()
    {
        //mainMenuButtons.Post(gameObject);
        Application.Quit();
    }

    IEnumerator rollCreds()
    {
        yield return new WaitForSeconds(3f);
        
        activeCredits = true;

        yield return null;
        
    }
}
