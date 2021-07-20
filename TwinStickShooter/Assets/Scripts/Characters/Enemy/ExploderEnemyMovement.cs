using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExploderEnemyMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private NavMeshAgent agent;

    public NavMeshAgent Agent
    {
        //set { agent = value; }
        get { return agent; }
    }

    private CharacterController characterController;
    [SerializeField]
    private float seekSpeed = 10f;
    [SerializeField]
    private float seekRadius = 10f;
    [SerializeField]
    private float explodeDistance = 2f;
    [SerializeField]
    private float explosionRadius = 4f;
    [SerializeField]
    private int explosionDamage = 10;
    [SerializeField]
    private float explosionTime = 3f;
    private float explosionTimer = 0f;
    private bool isExploding = false;
    [SerializeField]
    private float recheckPathPeriod = 10f;
    private float recheckPathTimer;
    
    private bool isPlayerInRange = false;
    public bool IsPlayerInRange
    {
        set { isPlayerInRange = value; }
    }

    private Material material;

    [SerializeField]
    private GameObject explosion;

    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
        recheckPathTimer = recheckPathPeriod;
        material = GetComponent<Renderer>().material;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        recheckPathTimer -= Time.deltaTime;
        if (!isExploding)
        {
            if (!isPlayerInRange)
            {
                if (recheckPathTimer <= 0f)
                {
                    Pathfind();
                }
            }
            else
            {
                Seek();
            }
        }
        else
        {
            //Debug.Log("Distance to Player: " + Vector3.Distance(transform.position, player.transform.position));
            Explode();
        }
    }

    public void Pathfind()
    {
        agent.destination = player.transform.position;
        recheckPathTimer = recheckPathPeriod;
    }

    public void Seek()
    {
        
        if (Vector3.Distance(transform.position, player.transform.position) > explodeDistance)
        {
            //Debug.Log("Seeking");
            //Debug.Log("Distance to Player: " + Vector3.Distance(transform.position, player.transform.position));
            Vector3 enemyToPlayer = player.transform.position - transform.position;
            enemyToPlayer.Normalize();
            enemyToPlayer *= seekSpeed * Time.deltaTime;

            characterController.Move(enemyToPlayer);
        }
        else
        {
            isExploding = true;
            /*Debug.Log("Exploding");
            Debug.Log("Distance to Player: " + Vector3.Distance(transform.position, player.transform.position));*/
        }
    }

    public void Explode()
    {
        material.SetColor("_EmissionColor", Color.white * (Mathf.Abs(Mathf.Sin(Mathf.Pow(4, Mathf.Lerp(0, explosionTime, explosionTimer / explosionTime))))));
        explosionTimer += Time.deltaTime;

        if(explosionTimer >= explosionTime)
        {
            CreateExplosion();
        }
    }
    public void CreateExplosion()
    {
        GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionInstance.GetComponent<Explosion>().Setup(explosionRadius, explosionDamage);
        Destroy(gameObject);
    }
}
