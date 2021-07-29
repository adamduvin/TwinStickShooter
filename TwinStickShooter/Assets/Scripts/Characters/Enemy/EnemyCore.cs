using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCore : CharacterCore
{
    private SpawnPoint spawnPoint;
    private SpawnController spawnController;

    public void SetSpawnPoint(SpawnPoint spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    public void SetSpawnController(SpawnController spawnController)
    {
        this.spawnController = spawnController;
    }

    public override void Die()
    {
        spawnController.HandleEnemyDeath();
        spawnPoint.GetNewEnemy();
        base.Die();

        Destroy(gameObject);
    }
}
