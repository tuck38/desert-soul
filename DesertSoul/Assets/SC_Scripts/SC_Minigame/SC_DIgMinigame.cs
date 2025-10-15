using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SC_DIgMinigame : MonoBehaviour
{
    [SerializeField] GameObject drillFloorPrefab;
    [SerializeField] Vector3[] drillFloorSpawnPoints;
    [SerializeField] Transform playerSpawn;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI highScoreText;

    bool gameStarted = false;
    float gameTime = 0.0f;
    GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            gameTime += Time.deltaTime;
            //timerText.text = gameTime.ToString();
        }
    }

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;
        gameTime = 0.0f;
        player = GameObject.Find("Player");
        player.transform.position = playerSpawn.position;
    }

    void GameOver()
    {
        gameStarted = false;
        for(int i = 0; i < drillFloorSpawnPoints.Length; i++)
        {
            Instantiate(drillFloorPrefab, drillFloorSpawnPoints[i], Quaternion.identity, transform).transform.localPosition = drillFloorSpawnPoints[i];
        }

        List<SC_Door> doors = new List<SC_Door>(FindObjectsByType<SC_Door>(FindObjectsSortMode.None));
        player.transform.position = doors[0].spawn.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameOver();
        }
    }
}
