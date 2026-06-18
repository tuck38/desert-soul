using UnityEngine;

public class SC_BossTrigger : MonoBehaviour
{

    [SerializeField] private SC_BossBase bossBase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        bossBase.StartFight();
    }
}
