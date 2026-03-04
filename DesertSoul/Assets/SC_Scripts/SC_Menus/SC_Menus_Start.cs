using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_Menus_Start : MonoBehaviour
{

    [SerializeField] string LastScene;
    [Header("Wwise Events")]
    public AK.Wwise.Event mainMenuButtons;
    void Start()
    {
        
    }

    public void StartGame ()
    {
        mainMenuButtons.Post(gameObject);
        SceneManager.LoadScene(LastScene);
    }

    public void Options()
    {
        mainMenuButtons.Post(gameObject);
    }

    public void QuitGame()
    {
        mainMenuButtons.Post(gameObject);
        Application.Quit();
    }
}
