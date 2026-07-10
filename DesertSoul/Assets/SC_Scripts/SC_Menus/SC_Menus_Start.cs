using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SC_Menus_Start : MonoBehaviour
{

    [SerializeField] string LastScene;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] RectTransform creditTransform;
    [SerializeField] GameObject mainScreen;
    public float credSpd;
    //[Header("Wwise Events")]
    //public AK.Wwise.Event mainMenuButtons;
    bool activeCredits = false;
    void Update()
    {
        if (activeCredits)
        {
            creditTransform.anchoredPosition += new Vector2(0, +credSpd * Time.deltaTime);
        }
    }

    public void StartGame ()
    {
       // mainMenuButtons.Post(gameObject);
        SceneManager.LoadScene(LastScene);
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
