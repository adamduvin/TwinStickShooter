using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private SpawnController spawnController;
    private Spawner spawner;
    private GameObject enemyPrefab;
    bool currentWaveIsNotEmpty;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponentInParent<Spawner>();
        spawnController = spawner.GetComponentInParent<SpawnController>();
        currentWaveIsNotEmpty = true;
        spawner.StartSpawning += GetNewEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetNewEnemy()
    {
        currentWaveIsNotEmpty = spawner.AssignEnemyToSpawnPoint(ref enemyPrefab);
        if (currentWaveIsNotEmpty)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            enemy.GetComponent<EnemyCore>().SetSpawnPoint(this);
            enemy.GetComponent<EnemyCore>().SetSpawnController(spawnController);
        }
    }
}
