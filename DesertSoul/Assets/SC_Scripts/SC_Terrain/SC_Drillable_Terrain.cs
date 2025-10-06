using UnityEngine;

public class SC_Drillable_Terrain : MonoBehaviour
{

    BoxCollider2D col;
    SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BreakTerrain()
    {
        col.enabled = false;
        //Special break terrain code for a cool visual effect
        sprite.enabled = false;
    }

    public void FixTerrain()
    {
        //Made this if we wanted to reset the terrain with day/night cycle
        col.enabled = true;
        sprite.enabled = true;
    }
}
