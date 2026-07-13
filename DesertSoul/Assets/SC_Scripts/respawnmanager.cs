using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.Timeline;
using UnityEngine;

public class respawnmanager : MonoBehaviour
{
    [SerializeField] SpriteRenderer rune1;

    [SerializeField] SpriteRenderer rune2;

    [SerializeField] float spawnTime;

    private float timeToSpawn = 0;

    [SerializeField] float timeToGlow;

    private float currentTimer = 0;

    private float currentGlow;

    [SerializeField] SC_Player_Move playerMove;

    [SerializeField] SpriteRenderer playerSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToGlow >= currentTimer)
        {
            currentTimer += Time.deltaTime;
            float percentegeComplete = currentTimer/timeToGlow;

            currentGlow = Mathf.Lerp(0, 1, percentegeComplete);

            rune1.color = new Color(rune1.color.r, rune1.color.g, rune1.color.r, currentGlow);
            rune2.color = new Color(rune1.color.r, rune1.color.g, rune1.color.r, currentGlow);

        }

        if(spawnTime >= timeToSpawn)
        {
            timeToSpawn += Time.deltaTime;
        }
        else
        {
            respawned();
        }

    }

    void Glow()
    {
        
    }

    public void respawned()
    {
        
        playerMove.SetCanMove(true);

        playerMove.isFirstRoom(false);

        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1);
    }
}
