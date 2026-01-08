using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LockRoom : MonoBehaviour
{

    [SerializeField] protected List<SC_Door> roomDoors;
    //may not need this
    [SerializeField] private Sprite doorLock;

    [SerializeField] private List<SC_EnemySpawner> spawners;

    [SerializeField] private List<SC_EnemyWave> waves;

    private int currentWave = 0;

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
        
    }

    void CloseRoom()
    {
        
    }

    void SpawnWave()
    {
        
    }

    void NextWave()
    {
        
    }

    void OpenRoom()
    {
        
    }

}
