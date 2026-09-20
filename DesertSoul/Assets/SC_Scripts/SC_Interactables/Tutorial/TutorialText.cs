using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public GameObject tutText;
    void Start()
    {
        tutText.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        tutText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        tutText.SetActive(false);
        Destroy(this);
    }
}
