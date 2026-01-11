using System;
using System.Collections.Generic;
using UnityEngine;

public class SC_EnemyWave : MonoBehaviour
{
    public int waveNum;
    [SerializeField] public List<GameObject> enemies;
    [SerializeField] public List<int> spawnerIDs;
}
