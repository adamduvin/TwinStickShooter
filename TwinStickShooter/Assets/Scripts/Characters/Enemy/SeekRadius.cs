using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekRadius : MonoBehaviour
{
    [SerializeField]
    private ExploderEnemyMovement exploderEnemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)         // Player Layer
        {
            exploderEnemyMovement.IsPlayerInRange = true;
            exploderEnemyMovement.Agent.isStopped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)         // Player Layer
        {
            exploderEnemyMovement.IsPlayerInRange = false;
            exploderEnemyMovement.Agent.isStopped = false;
            exploderEnemyMovement.Pathfind();
        }
    }
}
