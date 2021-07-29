using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploderEnemyCore : EnemyCore
{
    ExploderEnemyMovement exploderEnemyMovement;
    public override void Start()
    {
        exploderEnemyMovement = GetComponent<ExploderEnemyMovement>();
        base.Start();
    }

    public override void Die()
    {
        exploderEnemyMovement.CreateExplosion();
        base.Die();
    }
}
