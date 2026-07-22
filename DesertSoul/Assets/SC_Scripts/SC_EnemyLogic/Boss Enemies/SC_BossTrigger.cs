using UnityEngine;

public class SC_BossTrigger : MonoBehaviour
{

    [SerializeField] private SC_BossBase bossBase;

    [SerializeField] SpriteRenderer sprite;

    [SerializeField] BoxCollider2D box;
    [SerializeField] GameObject BossDoor;
    bool active = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(active)
        {
            bossBase.StartFight();
            BossDoor.SetActive(true);
            sprite.enabled = true;
        }
    }

    public void fightOver()
    {
        BossDoor.SetActive(false);
        sprite.enabled = true;
        //active = false;
    }
}
