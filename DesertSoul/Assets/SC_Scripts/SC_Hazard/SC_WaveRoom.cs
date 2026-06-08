using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_WaveRoom : MonoBehaviour
{

    [SerializeField] protected List<GameObject> roomDoors;
    //may not need this
    [SerializeField] private Sprite doorLock;

    [SerializeField] private List<SC_EnemySpawner> spawners;

    [SerializeField] private List<SC_EnemyWave> waves;

    private int currentWave = 0;

    private int currentEnemies = 0;

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
        if(collision.tag == "Player")
        {
            CloseRoom();
        }
    }

    void CloseRoom()
    {
        //does nothing rn, dont need it for demo
        for(int i = 0; i < roomDoors.Count; i++)
        {
            roomDoors[i].SetActive(true);
        }
        SpawnWave();
    }

    void SpawnWave()
    {
        Debug.Log("spawning wave");
        for(int i = 0; i < waves[currentWave].enemies.Count; i++)
        {
            Transform spawnTransform = null;
            //this sucks
            for(int j = 0; j < spawners.Count; j++)
            {
                if(spawners[j].spawnerID == waves[currentWave].spawnerIDs[i])
                {
                    spawnTransform = spawners[j].transform;
                }
                else
                {
                    //edge case
                }
            }

            Debug.Log("herm");
            GameObject enemy = Instantiate(waves[currentWave].enemies[i], new Vector3(spawnTransform.position.x, spawnTransform.position.y, spawnTransform.position.z), Quaternion.identity);
            currentEnemies++;

            //give enemy item
            enemy.GetComponent<SC_Enemy_Base>().setMommaSpawner(this);
        }
    }

    public void checkWave()
    {
        currentEnemies--;

        if(currentEnemies < 1)
        {
            Debug.Log("wave over");
            currentWave++;
            if(waves.Count <= currentWave)
            {
                OpenRoom();
            }
            else
            {
                SpawnWave();
            }
        }
    }

    void OpenRoom()
    {
                for(int i = 0; i < roomDoors.Count; i++)
        {
            roomDoors[i].SetActive(false);
        }
    }

}
