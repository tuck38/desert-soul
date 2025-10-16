using UnityEngine;

public class SC_CollectibleDiallogueTest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SC_DialogueManager.getInstance().testItemCount++;
            gameObject.SetActive(false);
        }
    }
}
