using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Globals;

public class CharacterCore : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private float health;
    public float Health
    {
        get { return health; }
    }
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private float healthLerpPercent;

    public virtual void Start()
    {
        health = maxHealth;
    }

    void Update()
    {

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, healthLerpPercent * Time.deltaTime); // Health divided by 100, but might need to change. Basically needs to be between 0.0 and 1.0.
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
