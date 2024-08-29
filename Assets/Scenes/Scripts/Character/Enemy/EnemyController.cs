using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterBase
{
    private float health = 100;

    public override void Attack()
    {
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    public override void Move()
    {
        
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Current Health : " + health);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
}
