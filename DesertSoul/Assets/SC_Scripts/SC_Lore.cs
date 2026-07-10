using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class SC_Lore : MonoBehaviour
{

    [SerializeField] string Lore;
    [SerializeField] TextMeshProUGUI loreTXT;
    [SerializeField] GameObject lorePanel;
    [SerializeField] GameObject gameInput;
    bool isInRange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loreTXT.text = Lore;
        lorePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            gameInput.SetActive(false);
            lorePanel.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            lorePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            gameInput.SetActive(true);
            isInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            gameInput.SetActive(false);
            lorePanel.SetActive(false);
            isInRange = false;
        }
    }

    public void Close()
    {
        lorePanel.SetActive(false);
    }
}
