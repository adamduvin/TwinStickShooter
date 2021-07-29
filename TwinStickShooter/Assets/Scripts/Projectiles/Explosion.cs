using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private int damage = 0;
    private float animTime = 1f;
    private float animTimer = 0f;
    private bool existedForOneFrame = false;
    private bool afterFirstFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(animTimer < animTime)
        {
            animTimer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    private void FixedUpdate()
    {
        // This prevents the explosion from double-dipping, so it can only do damage on the first physics update
        if (!existedForOneFrame)
        {
            if (afterFirstFrame)
            {
                existedForOneFrame = true;
            }
        }
        if (!afterFirstFrame)
        {
            afterFirstFrame = true;
        }
    }

    public void Setup(float size, int damage)
    {
        transform.localScale *= size;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (!existedForOneFrame)
        {
            //if()
            other.GetComponent<CharacterCore>().TakeDamage(damage);
        }
    }
}
