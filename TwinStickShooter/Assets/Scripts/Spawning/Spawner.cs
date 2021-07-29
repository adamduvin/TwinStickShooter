using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private byte spawnerID;
    private SpawnController spawnController;
    private Stack<GameObject> currentWave;
    private Stack<GameObject[]> waves;

    [SerializeField]
    private GameObject enemyPrefab;             // For development only, will be removed later

    // Start is called before the first frame update
    void Start()
    {

        waves = new Stack<GameObject[]>();
        currentWave = new Stack<GameObject>();
        // Hardcoded waves. Will be removed when Scriptable Object system is implemented
        GameObject[] wave1 = new GameObject[] { enemyPrefab };
        GameObject[] wave2 = new GameObject[] { enemyPrefab, enemyPrefab, enemyPrefab };
        GameObject[] wave3 = new GameObject[] { enemyPrefab, enemyPrefab, enemyPrefab, enemyPrefab, enemyPrefab };

        waves.Push(wave3);
        waves.Push(wave2);
        waves.Push(wave1);
        // End hardcoded values

        spawnController = GetComponentInParent<SpawnController>();
        spawnController.StartNewRound += PopulateEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignID(byte id)
    {
        spawnerID = id;
    }

    public void SetupWaves(Stack<GameObject[]> waves)
    {
        this.waves = waves;
    }

    public void PopulateEnemies()
    {
        if(waves.Count > 0)
        {
            GameObject[] nextWave = waves.Pop();
            for (int i = nextWave.Length - 1; i >= 0; i--)
            {
                currentWave.Push(nextWave[i]);
            }
            StartSpawning();
        }
    }

    public delegate void SpawnPointsStartRound();
    public event SpawnPointsStartRound StartSpawning;

    public bool AssignEnemyToSpawnPoint(ref GameObject enemy)
    {
        if(currentWave.Count > 0)
        {
            enemy = currentWave.Pop();
            return true;
        }
        return false;
    }
}
