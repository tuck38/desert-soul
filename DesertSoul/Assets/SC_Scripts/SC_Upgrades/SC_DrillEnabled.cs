using UnityEngine;
using System.Collections;

public class SC_DrillEnabled : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject unlockText;
    // Update is called once per frame

    private bool drill = false;

    void Awake()
    {
        
    }

    void Update()
    {
        if(drill)
        {
            GameManager.Instance.SetDrillActive(true);  
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            drill = true;
            unlockText.SetActive(true);
            StartCoroutine(UpgradeCelebrate());
        }

    }

    IEnumerator UpgradeCelebrate()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(4f);

        unlockText.SetActive(false);
        Destroy(gameObject);
        Time.timeScale = 1f;

        yield return null;
    }
}
