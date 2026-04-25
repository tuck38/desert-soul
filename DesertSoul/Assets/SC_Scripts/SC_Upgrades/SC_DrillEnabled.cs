using UnityEngine;
using System.Collections;

public class SC_DrillEnabled : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject unlockText;
    // Update is called once per frame

    void Awake()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.GetComponent<SC_UpgradeCheck>().isDrill = true;
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
