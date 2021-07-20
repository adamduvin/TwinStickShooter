using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Globals;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int impactDamage;
    [SerializeField]
    private float areaOfEffectDiameter;
    [SerializeField]
    private DamageType damageType;
    [SerializeField]
    private GameObject areaOfEffectPrototype;
    private Rigidbody rb;
    [SerializeField]
    private TrailRenderer trailRenderer;

    public GameObject Shooter { get; set; }

    private void Start()
    {

    }
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        if (impactDamage <= 0)
        {
            impactDamage = 1;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.gameObject.layer)
        {
            case 6:
                if(collision.gameObject != Shooter)
                {
                    collision.collider.GetComponent<PlayerCore>().TakeDamage(impactDamage);
                    //gameObject.SetActive(false);
                    Destroy(gameObject);
                }
                break;
            case 7:
                collision.collider.GetComponent<EnemyCore>().TakeDamage(impactDamage);
                //gameObject.SetActive(false);
                Destroy(gameObject);
                break;
            case 8:     // Only for greyboxing. This layer is the YuME_TileMap gameobjects and this may be removed later if greyboxing method changes
            case 9:
                //gameObject.SetActive(false);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 10:
                //gameObject.SetActive(false);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    public void Shoot(GameObject projectileSpawnPoint, Vector3 turretForward, float maxVelocity)
    {
        rb.transform.position = projectileSpawnPoint.transform.position;
        /*if (trailRenderer)
        {
            trailRenderer.Clear();
        }*/
        rb.transform.forward = turretForward;
        rb.velocity = rb.transform.forward.normalized * maxVelocity;
    }
}
