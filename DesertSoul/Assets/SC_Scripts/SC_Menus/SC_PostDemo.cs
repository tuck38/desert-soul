using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_PostDemo : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;

    private int timesPressed = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        if (timesPressed == 1)
        {
            text1.SetActive(false);
            text2.SetActive(true);
            timesPressed ++;
        }
        else if (timesPressed == 2)
        {
            text2.SetActive(false);
            text3.SetActive(true);
            timesPressed ++;
        }
        else if (timesPressed == 3)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}
