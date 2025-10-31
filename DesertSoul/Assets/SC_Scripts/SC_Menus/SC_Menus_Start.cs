using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_Menus_Start : MonoBehaviour
{

    [SerializeField] string LastScene;

    void Start()
    {
        
    }

    public void StartGame ()
    {
        SceneManager.LoadScene(LastScene);
    }

    public void Options()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
