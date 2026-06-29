using UnityEngine;

public class SC_BossTrigger : MonoBehaviour
{

    [SerializeField] private SC_BossBase bossBase;

    [SerializeField] SpriteRenderer sprite;

    [SerializeField] BoxCollider2D box;
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
            box.isTrigger = false;
            sprite.enabled = true;
        }
    }

    public void fightOver()
    {
        box.isTrigger = true;
        sprite.enabled = true;
        active = false;
    }
}
