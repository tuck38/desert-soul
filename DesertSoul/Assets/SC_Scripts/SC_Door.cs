using Unity.VisualScripting;
using UnityEngine;

public class SC_Door : MonoBehaviour
{
    BoxCollider2D doorTrigger;
    [SerializeField] public SC_Enum_Doors doorDir;
    [SerializeField] string nextScene;
    [SerializeField] public GameObject spawn;

    private void Awake()
    {
        doorTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.LoadNewLevel(nextScene, doorDir);
    }
}
